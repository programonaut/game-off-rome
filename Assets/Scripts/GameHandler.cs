using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameHandler : MonoBehaviour
{

    public static GameHandler Instance;

    [Title("Debug Settings")]
    [SerializeField] private bool spawnCards = true;

    [Title("Game Settings")]
    public bool isGameRunning = true;

    [OnValueChanged("CalculateCardInterval")]
    public float maxPlayTimeInSec = 60;
    public float startPlayTimeInSec = 40;
    [ReadOnly] public float buildingBuildTimeSum = 0f;
    [ReadOnly] public int buildingAmount = 0;

    public float currentPlayTimeInSec = 0;
    public float CurrentPlayTimeInSec {
        get {
            return currentPlayTimeInSec;
        }
        set {
            currentPlayTimeInSec = value;
            if (currentPlayTimeInSec > maxPlayTimeInSec)
                WonGame();
        }
    }

    public float[] slowdownAmountValues = {5,10,15,25};

    public float cardInterval = 0;
    [Button("Set Card Interval")]
    public void CalculateCardInterval() { cardInterval = maxPlayTimeInSec / 12f; } // 12 = cards every 2 hours
    public bool cardsActive = false;
    public int speed = 1;

    [Button]
    public void GetCityBuildTime() {
        BuildData[] buildings = FindObjectOfType<City>().buildings;
        buildingBuildTimeSum = buildings.Sum(group => group.buildElements.Sum(buildingElement => buildingElement.buildTimeInSec - (buildingElement.buildTimeInSec / 2))) * 1.5f;
        buildingAmount = buildings.Sum(group => group.buildElements.Length);
    }

    public int suspiciousRounds = 0;
    public bool check = true;

    public void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
        if (spawnCards)
            StartCoroutine(SpawnCards());
        StartCoroutine(CheckCaught());
    }

    void Update()
    {
        CurrentPlayTimeInSec += Time.deltaTime;
    }

    public void LostGame() {
        if (isGameRunning) {
            isGameRunning = false;
            UIHandler.Instance.LoseCity();
            PauseGame();
        }
    }

    public void LostGameCaught() {
        if (isGameRunning) {
            isGameRunning = false;
            UIHandler.Instance.LoseCaught();
            PauseGame();
        }
    }

    private void WonGame() {
        if (isGameRunning) {
            isGameRunning = false;
            UIHandler.Instance.Win();
            PauseGame();
        }
    }

    public void PauseGame() {
        Time.timeScale = 0;
    }

    public void ResumeGame() {
        Time.timeScale = speed;
    }

    public void SetSpeed(int speed) {
        this.speed = speed;
        Time.timeScale = speed;
    }

    IEnumerator SpawnCards() {
        Debug.Log("Start spawning cards");
        while (isGameRunning) {
            yield return new WaitForSeconds(cardInterval);
            Debug.Log("Spawn card");
            if (CardSystem.Instance.SpawnCards()) {
                UIHandler.Instance.backgroundCards.SetActive(true);
                cardsActive = true;
                PauseGame();
            }
            check = true;
        }
    }

    public void PlayCard() {
        UIHandler.Instance.backgroundCards.SetActive(false);
        CardSystem.Instance.RemoveCards();
        cardsActive = false;
        if (isGameRunning)
            ResumeGame();
    }

    IEnumerator CheckCaught() {
        while (isGameRunning) {
            yield return new WaitForSeconds(cardInterval / 8);
            if (check) {
                SuspicousnessSystem sus = SuspicousnessSystem.Instance;

                if (sus.Suspicousness >= sus.threshold) {
                    suspiciousRounds++;
                    if (suspiciousRounds >= sus.maxSuspiciousRounds || sus.Suspicousness >= 100) {
                        LostGameCaught();
                    }
                }
                else {
                    suspiciousRounds = 0;
                }
                check = false;
            }
        }
    }
}
