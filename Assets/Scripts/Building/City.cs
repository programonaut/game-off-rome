using UnityEngine;
using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;

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

    public BuildElement[] buildQueue = new BuildElement[0];
    [ReadOnly] public BuildElement[] builtBuildings = new BuildElement[0];
    // [SerializeField] private Connections[] connections = null;

    [ReadOnly, SerializeField] private float timeBetweenBuildings = 0f;
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
        buildQueue = buildings.SelectMany(group => group.buildElements).ToArray();

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
        while (buildQueue.Length > 0) {
            if (!pauseBuilding) {
                BuildElement building = buildQueue[0];
                building.StartMoveBuilding();
                float waitTime = building.buildTimeInSec - (building.buildTimeInSec / 2);

                yield return new WaitForSeconds(waitTime);
                builtBuildings = builtBuildings.Append(building).ToArray();
                buildQueue = buildQueue.Skip(1).ToArray();
            }
            else {
                yield return new WaitForSeconds(0.1f);
            }
        }
        GameHandler.Instance.LostGame();
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

    public void PauseBuilding(float time) {
        pauseBuilding = true;
        StartCoroutine(WaitTime(time));
        pauseBuilding = false;
    }
    IEnumerator WaitTime(float time) {
        yield return new WaitForSeconds(time);
    }
}