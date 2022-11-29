using System.Collections;
using System.Collections.Generic;
using Hellmade.Sound;
using UnityEngine;

public class SceneHandler : MonoBehaviour
{
    public GameObject menu;
    public GameObject settings;
    public GameObject loading;

    // sound
    public AudioClip musicAudioClip;

    private void Start() {
        menu.SetActive(true);
        settings.SetActive(false);
        loading.SetActive(false);

        var clip = EazySoundManager.PlayMusic(musicAudioClip, 0.5f, true, true, .5f, .5f);
    }

    public void ChangeScene(string sceneName)
    {
        StartCoroutine(LoadScene(sceneName));
    }

    public void Exit() {
        Application.Quit();
    }

    public void OpenSettings() {
        menu.SetActive(false);
        settings.SetActive(true);
    }

    public void OpenMenu() {
        menu.SetActive(true);
        settings.SetActive(false);
    }

    public IEnumerator LoadScene(string sceneName) {
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);

        loading.GetComponent<Loading>().perc = 0;

        menu.SetActive(false);
        settings.SetActive(false);
        loading.SetActive(true);
        while (!asyncLoad.isDone) {
            yield return null;
        }
    }
}
