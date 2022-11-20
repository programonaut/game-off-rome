using UnityEngine;
using Sirenix.OdinInspector;

public enum ActionType {
    Destroy,
    Blockade
}
[CreateAssetMenu(fileName = "Action", menuName = "Action", order = 0)]
public class Action : ScriptableObject {
    public ActionType type;
    [OnValueChanged("UpdateBuiding")]
    public Building affectedBuilding;
    [ReadOnly] public int buildingId;
    public Transform affectedTransform; // position to spawn the particle system (if empty use the building's transform)
    public GameObject effectParticleSystem; // most likely a particle system
    public int slowdownAmount; // negative value to speed up
    public int suspicionIncrease; // negative for decrease

    private void UpdateBuiding() {
        buildingId = affectedBuilding.id;
        affectedTransform = affectedBuilding.transform;
    }

    public void Execute() {
        Debug.Log(CheckIfBuilt());
        Building building = FindBuilding();
        IncreaseSuspicion();
        switch (type) {
            case ActionType.Destroy:
                DestroyBuilding();
                break;
            case ActionType.Blockade:
                // affectedBuilding.Blockade();
                break;
        }
    }

    public Building FindBuilding() {
        BuildElement[][] groupedCityBuildings = FindObjectOfType<City>().buildings;
        foreach (BuildElement[] buildings in groupedCityBuildings) {
            foreach (BuildElement building in buildings) {
                if (building.id == buildingId) {
                    return building.GetComponent<Building>();
                }
            }
        }
        return null;
    }

    public bool CheckIfBuilt() {
        BuildElement[] builtBuildings = FindObjectOfType<City>().builtBuildings;
        foreach (BuildElement building in builtBuildings) {
                if (building.id == buildingId) {
                    return true;
                }
        }
        return false;
    }

    public void DestroyBuilding() {
        FindObjectOfType<City>().DestroyBuilding(FindBuilding());
    }

    public void IncreaseSuspicion() {
        SuspicousnessSystem suspicousnessSystem = SuspicousnessSystem.Instance;
        suspicousnessSystem?.IncreaseSuspicousness(suspicionIncrease);
    }
}