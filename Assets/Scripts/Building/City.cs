using UnityEngine;
using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;

public class City : SerializedMonoBehaviour {
    public BuildElement[][] buildings = new BuildElement[0][];

    [ReadOnly] public BuildElement[] buildQueue = new BuildElement[0];
    [ReadOnly] public BuildElement[] builtBuildings = new BuildElement[0];
    // [SerializeField] private Connections[] connections = null;

    [ReadOnly, SerializeField] private float timeBetweenBuildings = 0f;

    private void Start() {
        var rng = new System.Random();
        rng.Shuffle(buildings);

        buildQueue = buildings.SelectMany(x => x).ToArray();

        float totalTime = GameHandler.Instance.startPlayTimeInSec;

        int buildingBuildTimeSum = buildings.Sum(building => building.Sum(buildingElement => buildingElement.buildTimeInSec));
        int buildingAmount = buildings.Sum(buildingGroups => buildingGroups.Length);
        timeBetweenBuildings = (totalTime - buildingBuildTimeSum) / buildingAmount;

        StartCoroutine(BuildCity());
    }


    IEnumerator BuildCity() {
        while (buildQueue.Length > 0) {
            BuildElement building = buildQueue[0];
            building.StartMoveBuilding();
            float waitTime = building.buildTimeInSec + timeBetweenBuildings;

            yield return new WaitForSeconds(waitTime);
            builtBuildings = builtBuildings.Append(building).ToArray();
            buildQueue = buildQueue.Skip(1).ToArray();
        }
        GameHandler.Instance.FinishCity();
    }

    public void DestroyBuilding(BuildElement building) {
        Debug.Log("Destroy building");
        BuildElement buildElement = builtBuildings.FirstOrDefault(build => build.id == building.id);
        if (buildElement != null) {
            buildElement.DestroyBuilding();
            builtBuildings = builtBuildings.Where(build => build.id != building.id).ToArray();
            StartCoroutine(ScheduleRebuild(building));
        }
    }

    IEnumerator ScheduleRebuild(BuildElement building) {
        yield return new WaitForSeconds(building.destroyTimeInSec);
        buildQueue = buildQueue.Append(building).ToArray();
    }
}