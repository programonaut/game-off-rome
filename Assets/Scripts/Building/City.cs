using UnityEngine;
using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using System.Collections.Generic;

public struct BuildData {
    public string name;
    public BuildElement[] buildElements;
}

public class City : SerializedMonoBehaviour {
    public static City Instance;

    [ListDrawerSettings(ShowIndexLabels = true, CustomAddFunction = "CustomAddFunction")]
    public BuildData[] buildings = new BuildData[1];
        [ListDrawerSettings(ShowIndexLabels = true)]
    public BuildElement[][] temp = new BuildElement[0][];
    public BuildElement[][] temp2 = new BuildElement[0][];
    public bool randomize = true;

    public List<BuildElement> buildQueue = new List<BuildElement>();
    [ReadOnly] public BuildElement[] builtBuildings = new BuildElement[0];
    public bool pauseBuilding = false;

    private BuildData CustomAddFunction() {
        BuildData data = new BuildData();
        data.name = "";
        data.buildElements = new BuildElement[0];
        return data;
    }

    // [Button]
    private void Rearrange() {
        for (int i = 0; i < temp.Length; i++) {
            for (int j = 0; j < buildings.Length; j++)
            {
                BuildData item = buildings[j];

                if (item.name == i.ToString()) {
                    item.buildElements = temp[i].Clone() as BuildElement[];
                    Debug.Log("Found " + i.ToString());
                }

                buildings[j] = item;
            }
        }
    }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        if (randomize)
            ShuffleBuildings();        

        buildQueue = buildings.SelectMany(group => group.buildElements).ToList();
        StartCoroutine(BuildCity());
    }

    private void ShuffleBuildings() {
        var rng = new System.Random();

        foreach (BuildData data in buildings) {
            rng.Shuffle(data.buildElements);
        }

        rng.Shuffle(buildings);
    }


    IEnumerator BuildCity() {
        while (buildQueue.Count > 0) {
            if (!pauseBuilding) {
                BuildElement building = buildQueue[0];
                building.StartMoveBuilding();
                float waitTime = building.buildTimeInSec - (building.buildTimeInSec / 2);

                yield return new WaitForSeconds(waitTime);
                builtBuildings = builtBuildings.Append(building).ToArray();
                buildQueue = buildQueue.Skip(1).ToList();
            }
            else {
                yield return new WaitForSeconds(0.1f);
            }
        }
        GameHandler.Instance.LostGame();
    }

    public void DestroyBuildings(int id) {
        // find all built buildings with id
        // destroy them
        BuildElement[] buildings = builtBuildings.Where(building => building.id == id).ToArray();
        int randomIndex = Random.Range(3, buildQueue.Count);
        foreach (var building in buildings) {
            building.DestroyBuilding();
        
            StartCoroutine(ScheduleRebuild(building, randomIndex));
        }
        // remove from built buildings
        builtBuildings = builtBuildings.Where(build => build.id != id).ToArray();
    }

    IEnumerator ScheduleRebuild(BuildElement building, int index = -1) {
        yield return new WaitForSeconds(building.destroyTimeInSec);
        // add to build queue at index or the end
        if (index != -1 && buildQueue.Count > index) {
            buildQueue.Insert(index, building);
        } else {
            buildQueue = buildQueue.Append(building).ToList();
        }
    }

    public void PauseBuilding(float time) {
        StartCoroutine(WaitTime(time));
    }
    IEnumerator WaitTime(float time) {
        pauseBuilding = true;
        yield return new WaitForSeconds(time);
        pauseBuilding = false;
    }
}