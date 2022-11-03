using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;

[System.Serializable]
public class Quest
{
    //public string questID;
    public string description;
    //public LiquidColor liquidColor;
    //public MixColor mixColor;
    public PotionInfo targetPotion;
    public int amountToDeliver;
    public int currentAmount = 0;

    public Quest(string desc, PotionInfo potion, int amount)
    {
        description = desc;
        //liquidColor = lC;
        //mixColor = mC;
        targetPotion = potion;
        amountToDeliver = amount;
    }
}


public class QuestsManager : MonoBehaviour
{
    [SerializeField] string[] questsDescriptionsPool;
    public string[] QuestsDescriptionsPool => questsDescriptionsPool;


    [SerializeField] GameObject questsHUB;
    [SerializeField] QuestUI questUI;
    public QuestUI _QuestUI => questUI;

    int questProgress=0;
    int questsCompleted = 0;
    Quest currentQuest;
    public Quest CurrentQuest
    {
        get => currentQuest;
        set => currentQuest = value;
    }

    public static QuestsManager instance;

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
        GameEvents.onOrderAccepted += SetCurrentQuest;
    }

    private void OnDisable()
    {
        GameEvents.onOrderAccepted -= SetCurrentQuest;
    }

    public void SetCurrentQuest(Quest q, float delay)
    {
        CurrentQuest = q;
        Debug.LogWarning("currentQuest TargetPotion = " + CurrentQuest.targetPotion.mixColor);
    }

    public void AddToCurrentQuest(LiquidColor mainFilledColor, LiquidColor secondFilledColor, MixClass resultMix)
    {
        if (CurrentQuest == null)
            return;

        if (CurrentQuest.targetPotion.liquidColor == mainFilledColor && secondFilledColor == LiquidColor.NULL ||
            (resultMix != null && CurrentQuest.targetPotion.mixColor == resultMix.mixColor))
        {
            CurrentQuest.currentAmount++;

            if (CurrentQuest.currentAmount < CurrentQuest.amountToDeliver)
            {
                _QuestUI.UpdateQuestProgressText(CurrentQuest.currentAmount);
                Debug.LogWarning("CURRENT QUEST PROGRESS = " + CurrentQuest.currentAmount + "/" + CurrentQuest.amountToDeliver);
            }
            else
            {
                _QuestUI.UpdateQuestProgressText(CurrentQuest.amountToDeliver);
                Debug.LogWarning(CurrentQuest.currentAmount + "/" + CurrentQuest.amountToDeliver + " yes! quest complete!");
                CompleteQuest();
            }

        }
    }

    public void CompleteQuest()
    {
        CurrentQuest = null;
        _QuestUI.ShowOrderComplete();

        _QuestUI.PlayQuestCompleteParticles();
        SoundsManager.instance.Play_QuestCompletedSound();
    }

}
