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
        [OnValueChanged("UpdateBuilding")]
    public ActionType type;

    [SerializeField] public bool needsToBeBuild = true;
    [OnValueChanged("UpdateBuilding")]
    public Building affectedBuilding;
    [Tooltip("Can be automatically be assigned by dragging building")] public int buildingId;

    [OnValueChanged("UpdateSpawnPosition")]
    public Transform affectedTransform; // position to spawn the particle system (if empty use the building's transform)
    [ReadOnly] public Vector3 spawnPosition;

    public GameObject effectParticleSystem; // most likely a particle system

    public SlowAmountType slowdownAmountType = SlowAmountType.Medium; // negative value to speed up
    public SuspicousnessAmountType suspicionIncrease = SuspicousnessAmountType.Medium; // negative for decrease
    public bool goodCard = false;

    private void UpdateBuilding() {
        if (affectedBuilding != null) {
            buildingId = affectedBuilding.id;
            affectedTransform = affectedBuilding.transform;
            spawnPosition = affectedTransform.position;
            needsToBeBuild = true;
        } else {
            needsToBeBuild = false;
        }
    }

    private void UpdateSpawnPosition() {
        if (affectedBuilding == null) {
            spawnPosition = Vector3.zero;
        }
        else if (affectedTransform == null) {
            spawnPosition = affectedBuilding.transform.position;
        }
        else {
            spawnPosition = affectedTransform.position;
        }
    }

    public void Execute() {
        Debug.Log("Executing action: " + name);

        if (!goodCard) {
            Building building = FindBuilding();
            switch (type) {
                case ActionType.Destroy:
                    SpawnEffect();
                    City.Instance.DestroyBuilding(building);
                    break;
                case ActionType.Other:
                    // affectedBuilding.Blockade();
                    break;
            }

            // check if caught
            if (SuspicousnessSystem.Instance.Caught()){
                Debug.Log("Caught");
                GameHandler.Instance.LostGame();
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

    public Building FindBuilding() {
        BuildData[] groupedCityBuildings = City.Instance.buildings;
        foreach (BuildData group in groupedCityBuildings) {
            foreach (BuildElement building in group.buildElements) {
                if (building.id == buildingId) {
                    return building.GetComponent<Building>();
                }
            }
        }
        return null;
    }

    public bool CheckIfBuilt() {
        if(!needsToBeBuild)
            return true;

        BuildElement[] builtBuildings = City.Instance.builtBuildings;
        foreach (BuildElement building in builtBuildings) {
                if (building.id == buildingId) {
                    return true;
                }
        }
        return false;
    }

    public void SpawnEffect() {
        if (effectParticleSystem != null && affectedTransform != null) {
            Instantiate(effectParticleSystem, spawnPosition, Quaternion.identity);
        }
    }
}
