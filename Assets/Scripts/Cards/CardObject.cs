using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public enum ActionType {
    Other,
    Destroy,
}

public enum SlowAmountType {
    Low = 0,
    Medium = 1,
    High = 2,
    Super = 3
}

[CreateAssetMenu(fileName = "Card", menuName = "Card", order = 0)]
public class CardObject : ScriptableObject {

    public string cardName;
    [TextArea] public string cardDescription;
    public Sprite cardBackground;
    public Sprite cardFrame;

    [Title("Card Action")]
    public ActionType type;

    [SerializeField] public bool needsToBeBuild = true;
    public int buildingId;

    [OnValueChanged("UpdateSpawnPosition")]
    public Transform affectedTransform; // position to spawn the particle system (if empty use the building's transform)
    [ReadOnly] public Vector3 spawnPosition;

    public GameObject effectParticleSystem; // most likely a particle system

    public SlowAmountType slowdownAmountType = SlowAmountType.Medium; // negative value to speed up
    public SuspicousnessAmountType suspicionIncrease = SuspicousnessAmountType.Medium; // negative for decrease
    public bool goodCard = false;

    private void UpdateSpawnPosition() {
        if (affectedTransform != null) {
            spawnPosition = affectedTransform.position;
        }
    }

    public void Execute() {
        Debug.Log("Executing action: " + name);

        if (!goodCard) {
            switch (type) {
                case ActionType.Destroy:
                    SpawnEffect();
                    City.Instance.DestroyBuildings(buildingId);
                    break;
                case ActionType.Other:
                    // affectedBuilding.Blockade();
                    break;
            }

            // check if caught
            if (SuspicousnessSystem.Instance.Caught()){
                GameHandler.Instance.LostGameCaught();
            }

            // if not increase suspicion by defined value
            SuspicousnessSystem.Instance.IncreaseSuspicousness(SuspicousnessSystem.Instance.suspiciosnessValues[(int)suspicionIncrease]);
            float slowdownAmount = GameHandler.Instance.slowdownAmountValues[(int)slowdownAmountType];
            if (slowdownAmount > 0)
                City.Instance.PauseBuilding(slowdownAmount);
        }
        else {
            SuspicousnessSystem.Instance.IncreaseSuspicousness(-SuspicousnessSystem.Instance.suspiciosnessValues[(int)suspicionIncrease]);
            Debug.Log($"{GameHandler.Instance.CurrentPlayTimeInSec}, {GameHandler.Instance.slowdownAmountValues[(int)slowdownAmountType]}");
            GameHandler.Instance.CurrentPlayTimeInSec = GameHandler.Instance.CurrentPlayTimeInSec - GameHandler.Instance.slowdownAmountValues[(int)slowdownAmountType];
        }

        GameHandler.Instance.PlayCard();
    }

    public bool CheckIfBuilt() {
        if(!needsToBeBuild)
            return true;

        bool allBuilt = false;
        BuildElement[] builtBuildings = City.Instance.builtBuildings;
        foreach (BuildElement building in builtBuildings) {
            if (building.id == buildingId) {
                allBuilt = true;
            }
        }

        List<BuildElement> buildingQueue = City.Instance.buildQueue;
        foreach (BuildElement building in buildingQueue) {
            if (building.id == buildingId) {
                allBuilt = false;
            }
        }

        return allBuilt;
    }

    public void SpawnEffect() {
        if (effectParticleSystem != null && affectedTransform != null) {
            Instantiate(effectParticleSystem, spawnPosition, Quaternion.identity);
        }
    }
}
