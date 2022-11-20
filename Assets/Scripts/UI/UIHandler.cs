using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIHandler : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI buildingsLeftText;

    // Start is called before the first frame update
    void Start()
    {
        
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
