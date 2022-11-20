using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildElement : MonoBehaviour
{
    [SerializeField] public int id;
    [SerializeField] private float startY = 0f;
    [SerializeField] private float endY = 0f;
    [SerializeField] private Transform buildingTransform = null;
    [SerializeField] public int buildTimeInSec = 1;
    [SerializeField] public int destroyTimeInSec = 1;

    private void Awake() {
        buildingTransform.position = new Vector3(buildingTransform.position.x, startY, buildingTransform.position.z);
    }

    public void StartMoveBuilding() {
        Debug.Log("StartMoveBuilding");
        StartCoroutine(MoveBuilding(startY, endY, buildTimeInSec));
    }

    public void DestroyBuilding() {
        Debug.Log("DestroyBuilding");
        StartCoroutine(MoveBuilding(endY, startY, destroyTimeInSec));
    }

    // Move the building from start to end position
    IEnumerator MoveBuilding(float startY, float endY, float timeInSec)
    {
        float elapsedTime = 0;
        while (elapsedTime < timeInSec)
        {
            buildingTransform.position = new Vector3(buildingTransform.position.x, Mathf.Lerp(startY, endY, (elapsedTime / timeInSec)), buildingTransform.position.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
