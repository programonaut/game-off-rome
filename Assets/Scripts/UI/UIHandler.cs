using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class UIHandler : MonoBehaviour
{
    public static UIHandler Instance;

    public TextMeshProUGUI timeText;

    public Transform clockPointer;

    public Image suspisionBar;
    public Gradient suspisionGradient;

    public Transform cardHolder;
    public GameObject backgroundCards;
    public GameObject backgroundMenues;
    public GameObject menu;
    public GameObject settings;
    public GameObject loading;

    public GameObject win;
    public GameObject loseCity;
    public GameObject loseCaught;

    [Title("Speed")]
    public GameObject[] speedIcons;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        backgroundCards.SetActive(false);
        backgroundMenues.SetActive(false);
        menu.SetActive(false);
        settings.SetActive(false);
        loading.SetActive(false);
        win.SetActive(false);
        loseCity.SetActive(false);
        loseCaught.SetActive(false);
    }

    private void Start() {
        SetSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        if (clockPointer != null)
            UpdateTime();

        // open menu on escape
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (menu.activeSelf)
            {
                GameHandler.Instance.ResumeGame();
                ExitMenu();
            }
            else
            {
                GameHandler.Instance.PauseGame();
                OpenMenu();
            }
        }
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
        backgroundMenues.SetActive(true);
        menu.SetActive(false);
        settings.SetActive(true);
    }

    public void OpenMenu() {
        backgroundMenues.SetActive(true);
        menu.SetActive(true);
        settings.SetActive(false);
    }

    public void ExitMenu() {
        backgroundMenues.SetActive(false);
        if (!GameHandler.Instance.cardsActive)
            GameHandler.Instance.ResumeGame();
        menu.SetActive(false);
        settings.SetActive(false);
    }

    public void LoadMenu() {
        GameHandler.Instance.ResumeGame();
        GameHandler.Instance.SetSpeed(1);
        StartCoroutine(LoadScene("Menu"));
    }

    public IEnumerator LoadScene(string sceneName) {
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);

        loading.GetComponent<Loading>().perc = 0;

        backgroundMenues.SetActive(true);
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
        backgroundMenues.SetActive(true);
        win.SetActive(true);
    }

    public void LoseCity() {
        backgroundMenues.SetActive(true);
        loseCity.SetActive(true);
    }

    public void LoseCaught() {
        backgroundMenues.SetActive(true);
        loseCaught.SetActive(true);
    }

    public void UpdateSpeed() {
        int speed = GameHandler.Instance.speed;
        speed = speed++ > speedIcons.Length - 1 ? 1 : speed++;
        GameHandler.Instance.SetSpeed(speed);
        SetSpeed();
    }

    public void SetSpeed() 
    {
        int speed = GameHandler.Instance.speed;
        for (int i = 0; i < speedIcons.Length; i++) {
            if (i != speed - 1)
                speedIcons[i].SetActive(false);
            else
                speedIcons[i].SetActive(true);
        }
    }
}
