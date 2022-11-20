using UnityEngine;
using Sirenix.OdinInspector;

public enum ActionType {
    Destroy,
    Blockade
}
[CreateAssetMenu(fileName = "Action", menuName = "Action", order = 0)]
public class Action : ScriptableObject {
    public ActionType type;
    [OnValueChanged("UpdateBuilding")]
    public Building affectedBuilding;
    [ReadOnly] public int buildingId;
    public Transform affectedTransform; // position to spawn the particle system (if empty use the building's transform)
    public GameObject effectParticleSystem; // most likely a particle system

    [HideIf("type", ActionType.Destroy)] public int slowdownAmount = 0; // negative value to speed up
    public int suspicionIncrease; // negative for decrease

    private void UpdateBuilding() {
        buildingId = affectedBuilding.id;
        affectedTransform = affectedBuilding.transform;
    }

    public void Execute() {
        Building building = FindBuilding();
        IncreaseSuspicion();
        switch (type) {
            case ActionType.Destroy:
                SpawnEffect();
                City.Instance.DestroyBuilding(building);
                break;
            case ActionType.Blockade:
                // affectedBuilding.Blockade();
                break;
        }
    }

    public Building FindBuilding() {
        BuildElement[][] groupedCityBuildings = City.Instance.buildings;
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
        BuildElement[] builtBuildings = City.Instance.builtBuildings;
        foreach (BuildElement building in builtBuildings) {
                if (building.id == buildingId) {
                    return true;
                }
        }
        return false;
    }

    public void IncreaseSuspicion() {
        SuspicousnessSystem suspicousnessSystem = SuspicousnessSystem.Instance;
        suspicousnessSystem?.IncreaseSuspicousness(suspicionIncrease);
    }

    public void SpawnEffect() {
        if (effectParticleSystem != null && affectedTransform != null) {
            Instantiate(effectParticleSystem, affectedTransform.position, Quaternion.identity);
        }
    }
}