using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public Story Story { get; private set; }
    [SerializeField] private List<Transform> gateLocationTransforms = new List<Transform>();

    public bool QuestActive = true;
    public bool CompletedStoryRead = false;

    public event Action QuestCompleted;

    public event Action<TextScriptableObject> ShowHint;
    [SerializeField] private GameObject questItem;

    [SerializeField] private AudioClip gateOpeningSound;
    private void Awake()
    {
        Story = GetComponent<Story>();
        questItem.SetActive(false);
    }

    public void QuestComplete()
    {        
        QuestActive = false;
        
        foreach (var gate in gateLocationTransforms)
        {
            TilemapManager.Instance.RemoveTile(gate.position);
        }

        questItem.SetActive(true);

        StartCoroutine(OpenGateAndCompleteQuest());
    }

    public void ShowDialogue()
    {
        if (QuestActive)        
            Story.ShowStory();            
        
        else        
            Story.ShowQuestCompletedStory();        
    }
    public void StoryCompleted()
    {
        if (QuestActive)
        {
            ShowHint?.Invoke(Story.GetHint());
        }
        HideDialogue();
    }
    public void WrongItemHandIn()
    {
        ShowHint?.Invoke(Story.GetHint());
        HideDialogue();
    }

    public void HideDialogue()
    {
        Story.EndStory();
    }

    private void OnDrawGizmos()
    {
        foreach (var gate in gateLocationTransforms)
        {
            Gizmos.DrawWireSphere(gate.position, 2);
        }
    }

    IEnumerator OpenGateAndCompleteQuest()
    {
        if (gateLocationTransforms.Count > 0)
        {
            AudioManager.Instance.StopNarration();
            AudioManager.Instance.PlaySFX(gateOpeningSound);
            yield return new WaitForSeconds(gateOpeningSound.length);
        }

        ShowDialogue();
    }
}
