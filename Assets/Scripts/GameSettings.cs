using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance;
    public float gameVolume = 0.5f;
    public Slider gameVolumeSlider;

    public void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        gameVolume = PlayerPrefs.GetFloat("gameVolume", 0.5f);
        Hellmade.Sound.EazySoundManager.GlobalVolume = gameVolume;
        gameVolumeSlider.value = gameVolume;
    }

    public void AdjustVolume(System.Single volume) {
        Hellmade.Sound.EazySoundManager.GlobalVolume = volume;
        gameVolume = volume;
        PlayerPrefs.SetFloat("gameVolume", volume);
    }
}
