using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public const int defaultCost = 10;
    [SerializeField] int moneyAmount = 200;
    public int MoneyAmount => moneyAmount;
    [SerializeField] MoneyUI moneyUI;



    public static MoneyManager instance;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        GameEvents.onFinishFlask += EarnMoney;
    }

    private void OnDisable()
    {
        GameEvents.onFinishFlask -= EarnMoney;
    }

    public void SpendMoney(int amount=defaultCost)
    {
        moneyAmount -= amount;
        moneyUI.UpdateMoneyText();
        moneyUI.PlayLostMoneyAnim();
    }

    public void EarnMoney(int v, LiquidColor l, int id)
    {
        //Debug.LogWarning("EARNED " + v);
        moneyAmount += Mathf.CeilToInt(v);
        moneyUI.UpdateMoneyText();
        moneyUI.PlayEarnedMoneyAnim();
    }

    public bool HasEnoughMoney()
    {
        return moneyAmount > 0;
    }
}
