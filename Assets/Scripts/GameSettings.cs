using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance;
    public float gameVolume = 0.5f;
    public Slider gameVolumeSlider;

    public TMP_Dropdown qualityDropdown;
    public int qualityIndex = 2;

    public TMP_Dropdown resolutionDropdown;
    public Resolution[] resolutions;
    public int resolutionIndex = 0;

    public Toggle fullscreenToggle;
    public bool fullscreen = true;

    public void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
        SetGameSettings();
    }

    public void SetGameSettings() {
        gameVolume = PlayerPrefs.GetFloat("gameVolume", 0.5f);
        Hellmade.Sound.EazySoundManager.GlobalVolume = gameVolume;
        gameVolumeSlider.value = gameVolume;

        qualityIndex = PlayerPrefs.GetInt("qualityIndex", 2);
        QualitySettings.SetQualityLevel(qualityIndex);
        qualityDropdown.value = qualityIndex;
        qualityDropdown.RefreshShownValue();


        resolutions = Screen.resolutions.GroupBy(r => new {r.width, r.height}).Select(r => r.First()).ToArray();
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(new List<string>(resolutions.Select(r => $"{r.width}x{r.height}").ToArray()));

        resolutionIndex = PlayerPrefs.GetInt("resolutionIndex", resolutions.Length - 1);
        resolutionDropdown.value = resolutionIndex;
        resolutionDropdown.RefreshShownValue();

        SetResolution(resolutionIndex);

        fullscreen = PlayerPrefs.GetInt("fullscreen", 1) == 1;
        SetFullscreen(fullscreen);
        fullscreenToggle.isOn = fullscreen;
    }

    public void AdjustVolume(System.Single volume) {
        Hellmade.Sound.EazySoundManager.GlobalVolume = volume;
        gameVolume = volume;
        PlayerPrefs.SetFloat("gameVolume", volume);
    }

    public void SetResolution(int index) {
        resolutionIndex = index;
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("resolutionIndex", index);
    }

    public void SetQuality(int index) {
        QualitySettings.SetQualityLevel(index);
        qualityIndex = index;
        PlayerPrefs.SetInt("qualityIndex", index);
    }

    public void SetFullscreen(bool fullscreen) {
        Screen.fullScreen = fullscreen;
        this.fullscreen = fullscreen;
        PlayerPrefs.SetInt("fullscreen", fullscreen ? 1 : 0);
    }
}
