using System.Collections;
using System.Collections.Generic;
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
    public int maxPlayTimeInSec = 60;
    public int startPlayTimeInSec = 40;

    private float currentPlayTimeInSec = 0;
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

    [ReadOnly] public float cardInterval = 0;
    [Button("Set Card Interval")]
    public void CalculateCardInterval() { cardInterval = maxPlayTimeInSec / 12f; } // 12 = cards every 2 hours

    public void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
        if (spawnCards)
            StartCoroutine(SpawnCards());
    }

    void Update()
    {
        CurrentPlayTimeInSec += Time.deltaTime;
    }

    public void LostGame() {
        if (isGameRunning) {
            isGameRunning = false;
            Debug.Log("Finish city -> Game Over");
            PauseGame();
        }
    }

    private void WonGame() {
        if (isGameRunning) {
            isGameRunning = false;
            Debug.Log("Time is over -> Game Won");
            PauseGame();
        }
    }

    public void PauseGame() {
        Time.timeScale = 0;
    }

    public void ResumeGame() {
        Time.timeScale = 1;
    }

    IEnumerator SpawnCards() {
        while (isGameRunning) {
            yield return new WaitForSeconds(cardInterval);
            if (CardSystem.Instance.SpawnCards())
                PauseGame();
        }
    }

    public void PlayCard() {
        ResumeGame();
        CardSystem.Instance.RemoveCards();
    }
}
