using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIHandler : MonoBehaviour
{
    public static UIHandler Instance;

    public TextMeshProUGUI timeText;
    public TextMeshProUGUI buildingsLeftText;

    public Transform cardHolder;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (timeText != null)
            timeText.text = $"{Mathf.RoundToInt(GameHandler.Instance.CurrentPlayTimeInSec)}/{Mathf.RoundToInt(GameHandler.Instance.maxPlayTimeInSec)}";

        if (buildingsLeftText != null)
            buildingsLeftText.text = $"{City.Instance.buildQueue.Length}";
    }
}
