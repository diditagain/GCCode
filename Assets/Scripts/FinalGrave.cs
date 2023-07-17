using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalGrave : MonoBehaviour
{
    private Quest quest;
    private Grave grave;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private float endScreenTime = 10;

    [SerializeField] private InputScriptableObject input;

    private void Awake()
    {
        grave = GetComponent<Grave>();
        quest = GetComponent<Quest>();
        grave.InteractionEnded += ShowEndScreen;
    }

    private void ShowEndScreen()
    {
        if(!quest.QuestActive)
            StartCoroutine(EndScreenCoroutine());
    }

    IEnumerator EndScreenCoroutine()
    {
        yield return new WaitForSeconds(2); 

        input.EnableControls(false);

        float timer = 0;
        endScreen.SetActive(true);
        while (timer < endScreenTime)
        {
            timer += Time.deltaTime;
            yield return null;  
        }
        endScreen.SetActive(false);

        input.EnableControls(true);
    }
}
