using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance { get; private set; }
    [SerializeField] private CanvasGroup blackScreen;
    [SerializeField] private AudioManager audioManager;

    public static event Action FadeInCompleted;
    public static event Action FadeOutCompleted;

    private void Awake()
    {
        if (Instance != null)
            Destroy(this);

        Instance = this;
        blackScreen.alpha = 1;
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }
    public void FadeIn()
    {
        StartCoroutine(FadeInCoroutine());
    }

    IEnumerator FadeInCoroutine()
    {
        blackScreen.alpha = 1;
        float timer = 0;
        while (timer < Constants.FadeTime)
        {
            float percent = timer / Constants.FadeTime;
            blackScreen.alpha = 1 - percent;
            audioManager.SetVolume(percent);

            timer += Time.deltaTime;
            yield return null;
        }
        FadeInCompleted?.Invoke();
    }

    IEnumerator FadeOutCoroutine()
    {
        blackScreen.alpha = 0;
        float timer = 0;
        while (timer < Constants.FadeTime)
        {
            float percent = timer / Constants.FadeTime;
            blackScreen.alpha = percent;
            audioManager.SetVolume(1 - percent);
            timer += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1);
        FadeOutCompleted?.Invoke();
    }
}
