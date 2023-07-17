using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject playerThoughtBubble;
    [SerializeField] private TextMeshProUGUI playerThoughtText;

    private Coroutine showHintCoroutine;
    private PlayerController _playerController;

    private void Awake()
    {
        playerThoughtBubble.SetActive(false);
        _playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowHint(TextScriptableObject hint)
    {
        if(showHintCoroutine == null)
            showHintCoroutine = StartCoroutine(TypeHintCoroutine(hint));

        playerThoughtBubble.SetActive(true);
    }
    public void HideHint()
    {
        if(showHintCoroutine != null)
            StopCoroutine(showHintCoroutine);

        showHintCoroutine = null;
        playerThoughtBubble.SetActive(false);

    }
    IEnumerator TypeHintCoroutine(TextScriptableObject hint)
    {
        playerThoughtText.text = string.Empty;
        AudioManager.Instance.PlayVoiceClip(hint.VoiceNarration);

        WaitForSeconds timePerCharacter = new WaitForSeconds(hint.VoiceNarration.length / hint.Text.Length);

        int currentCharIndex = 0;

        while (currentCharIndex <= hint.Text.Length)
        {
            string visibleText = hint.Text.Substring(0, currentCharIndex);
            string invisibleText = $"<color=#00000000>{hint.Text.Substring(currentCharIndex)}</color>";

            playerThoughtText.text = visibleText + invisibleText;

            currentCharIndex++;
            yield return timePerCharacter;
        }

        yield return new WaitForSeconds(Constants.HintDisplayTime);

        playerThoughtBubble.SetActive(false);
    }


    public void GoToNewArea(Area area)
    {
        _playerController.GoToNewArea(area);
    }
}

