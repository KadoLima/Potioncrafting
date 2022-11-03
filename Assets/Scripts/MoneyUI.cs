using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI moneyAmountText;

    Tween currentTween = null;


    // Start is called before the first frame update
    void Start()
    {
        UpdateMoneyText();
    }

    public void UpdateMoneyText()
    {
        moneyAmountText.text = "$" + MoneyManager.instance.MoneyAmount;
    }

    public void PlayLostMoneyAnim()
    {
        currentTween.Kill();
        currentTween = moneyAmountText.transform.DOLocalMove(moneyAmountText.transform.localPosition + new Vector3(0, -10, 0), .1f).SetLoops(2, LoopType.Yoyo);
    }

    public void PlayEarnedMoneyAnim()
    {
        currentTween.Kill();
        currentTween = moneyAmountText.transform.DOLocalMove(moneyAmountText.transform.localPosition + new Vector3(0,+20, 0), .1f).SetLoops(2, LoopType.Yoyo);
    }
}
