using UnityEngine;
using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;

public class City : SerializedMonoBehaviour {
    public BuildElement[][] buildings = new BuildElement[0][];
    [ReadOnly] public BuildElement[] builtBuildings = new BuildElement[0];
    // [SerializeField] private Connections[] connections = null;

    [SerializeField] private float totalTime = 0f;
    [SerializeField] private float lastOffset = 0f;

    private void Start() {
        var rng = new System.Random();

        Debug.Log("Shuffle buildings");
        rng.Shuffle(buildings);

        Debug.Log("Calculate total time");
        Calculate();
        Debug.Log("Total time: " + totalTime);
        StartCoroutine(BuildCity());
    }

    private void Calculate() {
        totalTime = buildings.Sum(building => building.Sum(build => build.buildTimeInSec));
    }

    private float CalculateOffset() {
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
                lastOffset = CalculateOffset();
                float waitTime = building.buildTimeInSec + lastOffset;
                if (index == buildings.Length - 1) {
                    waitTime = totalTime;
                }
                totalTime -= waitTime;

                yield return new WaitForSeconds(waitTime);
                builtBuildings = builtBuildings.Append(building).ToArray();
            }
        }
    }
}