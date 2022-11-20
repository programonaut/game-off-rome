using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Action action;

    private void Start() {
        Button button = GetComponentInChildren<Button>();
        button.onClick.AddListener(action.Execute);
    }
}
