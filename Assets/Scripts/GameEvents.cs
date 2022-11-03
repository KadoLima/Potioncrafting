using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents:MonoBehaviour
{

    public static event Action<int> onMoneyChanged;
    public static void MoneyChanged(int amount)
    {
        onMoneyChanged?.Invoke(amount);
    }

    public static event Action<int, LiquidColor, int> onFinishFlask;
    public static void FinishFlask(int moneyToEarn, LiquidColor liquid, int flaskID)
    {
        onFinishFlask?.Invoke(moneyToEarn, liquid, flaskID);
    }

    public static event Action<Quest, float> onOrderAccepted;

    public static void AcceptOrder(Quest quest, float delayToHide)
    {
        onOrderAccepted?.Invoke(quest,delayToHide);
    }
}
