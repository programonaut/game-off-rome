using UnityEngine;
using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;

public class City : SerializedMonoBehaviour {
    // Serialize two dimensional array of buildings
    [TableMatrix()]
    public GameObject[][] cityObjects = null;
    [SerializeField] private BuildElement[][] buildings = null;
    // [SerializeField] private Connections[] connections = null;

    [SerializeField] private float totalTime = 0f;
    [SerializeField] private float lastOffset = 0f;

    private void Awake() {
        // buildings = cityObjects.Select(x => x.GetComponent<BuildElement>()).ToArray();
        var rng = new System.Random();
        rng.Shuffle(cityObjects);
        buildings = cityObjects.Select(x => x.Select(y => y.GetComponent<BuildElement>()).ToArray()).ToArray();
        // totalTime = buildings.Sum(building => building.timeInSec);
        totalTime = buildings.Sum(building => building.Sum(build => build.timeInSec));
    }

    private void Start() {
        StartCoroutine(BuildCity());
    }

    private float calculateOffset() {
        if (lastOffset <= 0) {
            return Random.Range(1, 11) / 10f;
        } else {
            return -lastOffset;
        }
    }

    IEnumerator BuildCity() {
        foreach ((BuildElement[] buildings, int index) in buildings.Select((item, index) => (item, index))) {
            foreach (BuildElement building in buildings) {
                building.StartMoveBuilding();
                lastOffset = calculateOffset();
                float waitTime = building.timeInSec + lastOffset;
                if (index == buildings.Length - 1) {
                    waitTime = totalTime;
                }
                totalTime -= waitTime;

                yield return new WaitForSeconds(waitTime);
            }
        }
    }
}