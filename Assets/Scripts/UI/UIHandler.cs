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
    public GameObject menu;
    public GameObject settings;
    public GameObject loading;

    public GameObject win;
    public GameObject loseCity;
    public GameObject loseCaught;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        backgroundCardsAndMenues.SetActive(false);
        menu.SetActive(false);
        settings.SetActive(false);
        loading.SetActive(false);
        win.SetActive(false);
        loseCity.SetActive(false);
        loseCaught.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (buildingsLeftText != null)
            buildingsLeftText.text = $"{City.Instance.buildQueue.Count}";

        if (clockPointer != null)
            UpdateTime();

        // open menu on escape
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (menu.activeSelf)
                GameHandler.Instance.ResumeGame();
            else
                GameHandler.Instance.PauseGame();
            OpenMenu();
        }
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

    public void OpenSettings() {
        ShowCardsAndMenues();
        menu.SetActive(false);
        settings.SetActive(true);
    }

    public void OpenMenu() {
        ShowCardsAndMenues();
        menu.SetActive(true);
        settings.SetActive(false);
    }

    public void ExitMenu() {
        HideCardsAndMenues();
        GameHandler.Instance.ResumeGame();
        menu.SetActive(false);
        settings.SetActive(false);
    }

    public void LoadMenu() {
        StartCoroutine(LoadScene("Menu"));
    }

    public IEnumerator LoadScene(string sceneName) {
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);

        loading.GetComponent<Loading>().perc = 0;

        ShowCardsAndMenues();
        menu.SetActive(false);
        settings.SetActive(false);
        loading.SetActive(true);
        win.SetActive(false);
        loseCity.SetActive(false);
        loseCaught.SetActive(false);

        while (!asyncLoad.isDone) {
            yield return null;
        }
    }

    public void Win() {
        ShowCardsAndMenues();
        win.SetActive(true);
    }

    public void LoseCity() {
        ShowCardsAndMenues();
        loseCity.SetActive(true);
    }

    public void LoseCaught() {
        ShowCardsAndMenues();
        loseCaught.SetActive(true);
    }
}
