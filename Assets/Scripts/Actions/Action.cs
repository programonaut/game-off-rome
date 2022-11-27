using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;

public enum ActionType {
    Destroy,
    Blockade
}
[CreateAssetMenu(fileName = "Action", menuName = "Action", order = 0)]
public class Action : ScriptableObject {
    [OnValueChanged("UpdateBuilding")]
    public ActionType type;

    [SerializeField] private bool needsToBeBuild = true;
    [OnValueChanged("UpdateBuilding")]
    public Building affectedBuilding;
    [Tooltip("Can be automatically be assigned by dragging building")] public int buildingId;

    [OnValueChanged("UpdateSpawnPosition")]
    public Transform affectedTransform; // position to spawn the particle system (if empty use the building's transform)
    [ReadOnly] public Vector3 spawnPosition;

    public GameObject effectParticleSystem; // most likely a particle system

    [DisableIf("type", ActionType.Destroy)] public float slowdownAmount = 0; // negative value to speed up
    public int suspicionIncrease; // negative for decrease

    private void UpdateBuilding() {
        if (affectedBuilding != null) {
            buildingId = affectedBuilding.id;
            affectedTransform = affectedBuilding.transform;
            spawnPosition = affectedTransform.position;
            needsToBeBuild = true;
        } else {
            needsToBeBuild = false;
        }
        CalculateExtraTime();
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

    public void CalculateExtraTime() {
        if (affectedBuilding != null && type == ActionType.Destroy) {
            BuildElement buildElement = affectedBuilding.GetComponent<BuildElement>();
            slowdownAmount = buildElement.buildTimeInSec + buildElement.destroyTimeInSec;
        }
        else if (affectedBuilding == null && type == ActionType.Destroy) {
            slowdownAmount = 0;
        }
    }

    public void Execute() {
        Debug.Log("Executing action: " + name);

        Building building = FindBuilding();
        switch (type) {
            case ActionType.Destroy:
                SpawnEffect();
                City.Instance.DestroyBuilding(building);
                break;
            case ActionType.Blockade:
                // affectedBuilding.Blockade();
                break;
        }

        GameHandler.Instance.PlayCard();
        if (slowdownAmount > 0)
            City.Instance.PauseBuilding(slowdownAmount);

        // check if caught
        if (SuspicousnessSystem.Instance.Caught()){
            Debug.Log("Caught");
            GameHandler.Instance.LostGame();
        }

        // if not increase suspicion by defined value
        SuspicousnessSystem.Instance.IncreaseSuspicousness(suspicionIncrease);
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