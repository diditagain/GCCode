using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanel : MonoBehaviour
{
    public Selectable DefaultSelection;
    public void SetActive(bool active)
    {
        gameObject.SetActive(active);

        if (DefaultSelection != null && active)
            StartCoroutine(SelectDefaultButtonNextFrame());
    }

   IEnumerator SelectDefaultButtonNextFrame()
    {
        yield return null;
        DefaultSelection.Select();
    }
}
