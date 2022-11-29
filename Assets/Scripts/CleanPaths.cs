using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using PathCreation;

public class CleanPaths : MonoBehaviour
{
    [Button]
    void KeepChildAndDeletePath() {

    }

    [Button]
    void MoveChildToParentAndDeactivate() {
        PathCreator[] paths = FindObjectsOfType<PathCreator>();
        foreach (PathCreator path in paths) {
            if (path.transform.childCount > 0) {
                path.transform.GetChild(0).SetParent(path.transform.parent);
            }
            path.gameObject.SetActive(false);
        }
    }

    [Button]
    void ReactivateAllPaths() {
        PathCreator[] paths = FindObjectsOfType<PathCreator>(true);
        foreach (PathCreator path in paths) {
            path.gameObject.SetActive(true);
        }
    }
}
