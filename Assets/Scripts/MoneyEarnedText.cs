using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class MoneyEarnedText : MonoBehaviour
{
    [SerializeField]float originalPositionY;
    [SerializeField] float tweenTime = 0.5f;
    [SerializeField]TextMeshProUGUI valueText;
    [SerializeField]int flaskID;

    private void Start()
    {
        valueText.text = "";
    }

    private void OnEnable()
    {
        GameEvents.onFinishFlask += Show;
    }

    private void OnDisable()
    {
        GameEvents.onFinishFlask -= Show;
    }

    public void Show(int v, LiquidColor l, int id)
    {
        if (id == this.flaskID)
        {
            valueText.DOColor(new Color(valueText.color.r, valueText.color.g, valueText.color.b, 1), 0);
            valueText.text = "+$" + v.ToString();
            RectTransform r = GetComponent<RectTransform>();
            r.anchoredPosition = new Vector2(r.anchoredPosition.x, originalPositionY);
            valueText.DOColor(new Color(valueText.color.r, valueText.color.g, valueText.color.b, 0), tweenTime);
            r.DOAnchorPosY(originalPositionY + 20, tweenTime).OnComplete(DisableGO);
            r.DOScale(1.4f, tweenTime);
        }
    }

    void DisableGO()
    {
        RectTransform r = GetComponent<RectTransform>();
        r.DOScale(1f, 0f);
        //this.gameObject.SetActive(false);
    }
}
