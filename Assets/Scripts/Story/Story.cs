using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Story : MonoBehaviour
{
    [SerializeField] private UIPanel textBoxPanel;
    [SerializeField] private TextMeshProUGUI textBox;
    [SerializeField] private List<TextScriptableObject> storyParagraphs = new List<TextScriptableObject>();
    [SerializeField] private TextScriptableObject hint;
    [SerializeField] private List<TextScriptableObject> questCompletedParagraphs = new List<TextScriptableObject>();

    private List<TextScriptableObject> currentScript;

    private int currentParagraphIndex = 0;

    private bool storyInProgress = false;

    private Grave grave;
    private Quest quest;

    private void Awake()
    {
        textBoxPanel.SetActive(false);
        grave = GetComponent<Grave>();
        quest = GetComponent<Quest>();
    }

    public void ShowStory()
    {
        if (storyInProgress) return;
        currentScript = storyParagraphs;
        StartStory(storyParagraphs);
    }
    public void ShowQuestCompletedStory()
    {
        if (storyInProgress) return;
        
        StartStory(questCompletedParagraphs);
    }

    private void StartStory(List<TextScriptableObject> story)
    {
        currentScript = story;
        storyInProgress = true;
        textBoxPanel.SetActive(true);

        currentParagraphIndex = 0;
        StartCoroutine(TypeStoryCoroutine());
    }

    public void EndStory()
    {
        textBoxPanel.SetActive(false);
        textBox.text = string.Empty;
        storyInProgress = false;

        StopAllCoroutines();
        currentParagraphIndex = 0;
    }

    public void NextPage()
    {
        if (!storyInProgress) return;

        if (currentParagraphIndex == currentScript.Count - 1)
        {
            AudioManager.Instance.StopNarration();
            quest.CompletedStoryRead = true;
            quest.StoryCompleted();
            grave.StoryCompleted();            
            return;
        }

        StopAllCoroutines();
        currentParagraphIndex++;
        StartCoroutine(TypeStoryCoroutine());
    }
    public void PreviousPage()
    {
        if(!storyInProgress) return;

        if (currentParagraphIndex == 0)
            return;
        
        StopAllCoroutines();
        currentParagraphIndex--;
        textBox.text = currentScript[currentParagraphIndex].Text;
        AudioManager.Instance.PlayVoiceClip(currentScript[currentParagraphIndex].VoiceNarration);
    }

    public TextScriptableObject GetHint()
    {
        return hint;
    }

    

    IEnumerator TypeStoryCoroutine()
    {
        if(currentScript == null || currentScript.Count == 0)
            yield break;

        AudioManager.Instance.PlayVoiceClip(currentScript[currentParagraphIndex].VoiceNarration);

        textBox.text = string.Empty;
        string textToWrite = currentScript[currentParagraphIndex].Text;

        WaitForSeconds timePerCharacter = new WaitForSeconds(
            currentScript[currentParagraphIndex].VoiceNarration.length * 0.75f 
            / textToWrite.Length);
        
        int currentCharIndex = 0;

        while(currentCharIndex <= textToWrite.Length)
        {
            string visibleText = textToWrite.Substring(0, currentCharIndex);
            string invisibleText = $"<color=#00000000>{textToWrite.Substring(currentCharIndex)}</color>";

            textBox.text = visibleText + invisibleText;

            currentCharIndex++;
            yield return timePerCharacter;
        }
    }
}
