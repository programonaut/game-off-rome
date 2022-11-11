using UnityEngine;
using System.Collections;
using System.Linq;

public class City : MonoBehaviour {
    [SerializeField] private Building[] buildings = null;

    private float totalTime = 0f;
    private float lastOffset = 0f;

    private void Awake() {
        totalTime = buildings.Sum(building => building.timeInSec);
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
        foreach ((Building building, int index) in buildings.Select((item, index) => (item, index))) {
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