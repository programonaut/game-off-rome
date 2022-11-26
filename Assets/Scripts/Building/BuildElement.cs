using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class BuildElement : MonoBehaviour
{
    [SerializeField] public int id;
    [SerializeField] private float startY = 0f;
    [SerializeField] private float endY = 0f;
    [SerializeField] private Transform buildingTransform = null;
    [SerializeField] public float buildTimeInSec = 1;
    [SerializeField] public float destroyTimeInSec = 1;
    [Button]
    void SetChildTransform() {
        // iterate over all children and find the first active object
        for (int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i).gameObject.activeSelf) {
                buildingTransform = transform.GetChild(i);
                break;
            }
        }
    }

    private void Awake() {
        buildingTransform.localPosition = new Vector3(buildingTransform.localPosition.x, startY, buildingTransform.localPosition.z);
    }

    public void StartMoveBuilding() {
        StartCoroutine(MoveBuilding(startY, endY, buildTimeInSec));
    }

    public void DestroyBuilding() {
        StartCoroutine(MoveBuilding(endY, startY, destroyTimeInSec));
    }

    // Move the building from start to end position
    IEnumerator MoveBuilding(float startY, float endY, float timeInSec)
    {
        float elapsedTime = 0;
        while (elapsedTime < timeInSec)
        {
            buildingTransform.localPosition = new Vector3(buildingTransform.localPosition.x, Mathf.Lerp(startY, endY, (elapsedTime / timeInSec)), buildingTransform.localPosition.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
