using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishButton : MonoBehaviour
{
    [SerializeField] GameObject mixInner;
    [SerializeField] GameObject finishInner;

    public void ShowFinishInner()
    {
        finishInner.SetActive(true);
        mixInner.SetActive(false);
    }

    public void ShowMixInner()
    {
        finishInner.SetActive(false);
        mixInner.SetActive(true);
    }

    public void HideButton()
    {
        mixInner.SetActive(false);
        finishInner.SetActive(false);
    }
}
