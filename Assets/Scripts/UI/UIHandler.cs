using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public static UIHandler Instance;

    public TextMeshProUGUI timeText;
    public TextMeshProUGUI buildingsLeftText;

    public Transform clockPointer;

    public Image suspisionBar;
    public Gradient suspisionGradient;

    public Transform cardHolder;
    public GameObject backgroundCardsAndMenues;

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
        if (buildingsLeftText != null)
            buildingsLeftText.text = $"{City.Instance.buildQueue.Count}";

        if (clockPointer != null)
            UpdateTime();
    }

    public void ShowCardsAndMenues() {
        backgroundCardsAndMenues.SetActive(true);
    }

    public void HideCardsAndMenues() {
        backgroundCardsAndMenues.SetActive(false);
    }

    public void UpdateTime() {
        float maxTime = GameHandler.Instance.maxPlayTimeInSec;
        float currentTime = GameHandler.Instance.CurrentPlayTimeInSec;

        float perc = currentTime / maxTime;
        float maxRot = -360;

        clockPointer.localRotation = Quaternion.Euler(0, 0, maxRot * perc);

        var hours = Mathf.FloorToInt(currentTime / maxTime * 24);
        var minutes = Mathf.FloorToInt(((currentTime / maxTime * 24) - hours) * 60);
        var preHour = hours < 10 ? "0" : "";
        var preMinute = minutes < 10 ? "0" : "";
        timeText.text = $"{preHour}{hours}:{preMinute}{minutes}";
    }

    public void UpdateSuspicion() {
        int maxSuspicion = 100;
        int currentSuspicion = SuspicousnessSystem.Instance.Suspicousness;

        float perc = currentSuspicion / (float)maxSuspicion;

        if (suspisionBar != null)
        {
            suspisionBar.fillAmount = perc;
            suspisionBar.color = suspisionGradient.Evaluate(perc);
        }    
    }
}
