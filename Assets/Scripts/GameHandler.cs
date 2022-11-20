using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance;

    public bool isGameRunning = true;

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
                EndGame();
        }
    }

    public void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        CurrentPlayTimeInSec += Time.deltaTime;
    }

    public void FinishCity() {
        isGameRunning = false;
        Debug.Log("Finish city -> Game Over");
    }

    private void EndGame() {
        isGameRunning = false;
        Debug.Log("Time is over -> Game Won");
    }

    public void PauseGame() {
        Time.timeScale = 0;
    }

    public void ResumeGame() {
        Time.timeScale = 1;
    }
}
