using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private UIPanel settingsMenu;
    private bool settingsMenuOpen = false;

    [SerializeField] private CanvasGroup blackScreen;

    [SerializeField] private Selectable DefaultSelectedButton;
    [SerializeField] private AudioSource mainMenuAudioSource;

    private void Awake()
    {
        DefaultSelectedButton.Select();
        blackScreen.alpha = 0;
    }

    public void ToggleSettingsMenu()
    {
        if (settingsMenuOpen)
            DefaultSelectedButton.Select();

        settingsMenuOpen = !settingsMenuOpen;
        settingsMenu.SetActive(settingsMenuOpen);

    }

    public void PlayGame()
    {
        StartCoroutine(LoadGame());
    }

    IEnumerator LoadGame()
    {
        float timer = 0;
        while(timer < Constants.FadeTime)
        {
            float percent = timer / Constants.FadeTime;
            blackScreen.alpha = percent;
            mainMenuAudioSource.volume = 1 - percent;
            timer += Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene(1);
    }
}
