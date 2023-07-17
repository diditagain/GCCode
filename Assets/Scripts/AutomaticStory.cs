using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AutomaticStory : MonoBehaviour
{
    [SerializeField] private List<TextScriptableObject> paragraphs = new List<TextScriptableObject>();
    [SerializeField] private GameObject textBoxGO;
    [SerializeField] private TextMeshProUGUI textBox;
    [SerializeField] private AudioClip voiceClip;

    [SerializeField] List<Vector2> paragraphTimings; 

    private bool typing = false;
    private Coroutine storyCoroutine;
    private Coroutine typingCoroutine;
    [SerializeField] private LayerMask playerLayer;

    private void Awake()
    {
        textBoxGO.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int isPlayer = (int)Mathf.Pow(2, collision.gameObject.layer) & playerLayer;
        if (isPlayer == 0)
            return;

        if (storyCoroutine != null)
            return;

        
        textBoxGO.SetActive(true);
        StartCoroutine(StartStory());

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        int isPlayer = (int)Mathf.Pow(2, collision.gameObject.layer) & playerLayer;
        if (isPlayer == 0)
            return;

        StopStory();

    }

    private void StopStory()
    {
        StopAllCoroutines();
        storyCoroutine = null;

        textBoxGO.SetActive(false);
        textBox.text = string.Empty;
        
        AudioManager.Instance.StopNarration();        
    }

    IEnumerator StartStory()
    {
        yield return new WaitForSeconds(1);

        AudioManager.Instance.PlayVoiceClip(voiceClip);
        int lastParagraphIndex = paragraphs.Count - 1;
        float endTime = paragraphTimings[lastParagraphIndex].y;

        int currentParagraph = 0;

        ////start first paragraph
        //float time = paragraphTimings[currentParagraph].y - paragraphTimings[currentParagraph].x;
        //float strLen = paragraphs[currentParagraph].Text.Length;
        //typingCoroutine = StartCoroutine(TypingCoroutine(paragraphs[currentParagraph].Text, new WaitForSeconds(time / strLen)));

        float timer = 0;
        while (timer < endTime)
        {
            if(timer >= paragraphTimings[currentParagraph].x && typingCoroutine == null)
            {      
                float time = paragraphTimings[currentParagraph].y - paragraphTimings[currentParagraph].x;
                float strLen = paragraphs[currentParagraph].Text.Length;
                typingCoroutine = StartCoroutine(TypingCoroutine(paragraphs[currentParagraph].Text, new WaitForSeconds(time / strLen)));
            }

            if(timer > paragraphTimings[currentParagraph].y)
            {                
                currentParagraph++;
                //time = paragraphTimings[currentParagraph].y - paragraphTimings[currentParagraph].x;
                //strLen = paragraphs[currentParagraph].Text.Length;
                //typingCoroutine = StartCoroutine(
                //    TypingCoroutine(paragraphs[currentParagraph].Text, 
                //    new WaitForSeconds(time / strLen))
                //    );                
            }

            //if (timer < oneEnd && timer >= 0)
            //{
            //    if (currentParagraph != 0)
            //    {
            //        currentParagraph = 0;
            //        StartCoroutine(TypingCoroutine(currentParagraph, oneEnd));
            //    }
            //}
            //else if (timer < twoEnd && timer >= oneEnd)
            //{
            //    if (currentParagraph != 1)
            //    {
            //        currentParagraph = 1;
            //        StartCoroutine(TypingCoroutine(currentParagraph, twoEnd - oneEnd));
            //    }
            //}
            //else if (timer < threeEnd && timer >= twoEnd)
            //{
            //    if (currentParagraph != 2)
            //    {
            //        currentParagraph = 2;
            //        StartCoroutine(TypingCoroutine(currentParagraph, threeEnd - twoEnd));
            //    }
            //}
            //else if (timer < fourEnd && timer >= threeEnd)
            //{
            //    if (currentParagraph != 3)
            //    {
            //        currentParagraph = 3;
            //        StartCoroutine(TypingCoroutine(currentParagraph, fourEnd - threeEnd));
            //    }
            //}
            //else
            //{
            //    yield break;
            //}

            timer += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(2);
        StopStory();
    }

    IEnumerator TypingCoroutine(string text, WaitForSeconds timePerCharacter)
    {
        textBox.text = string.Empty;

        int textLength = text.Length;
        int currentCharIndex = 0;
                
        while (currentCharIndex <= textLength)
        {
            string visibleText = text.Substring(0, currentCharIndex);
            string invisibleText = $"<color=#00000000>{text.Substring(currentCharIndex)}</color>";

            textBox.text = visibleText + invisibleText;

            currentCharIndex++;
            yield return timePerCharacter;
        }
        typingCoroutine = null;
    }


}
