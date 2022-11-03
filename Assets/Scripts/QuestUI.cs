using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class QuestUI : MonoBehaviour
{
    [SerializeField] GameObject takeOrder;
    [SerializeField] GameObject orderComplete;
    [SerializeField] GameObject questProgress;
    [SerializeField] TextMeshProUGUI questProgressText;
    [SerializeField] ParticleSystem questCompleteParticles;
    Quest trackedQuest;

    private void Start()
    {
        ShowTakeOrder();
    }

    private void OnEnable()
    {
        GameEvents.onOrderAccepted += ShowOrderProgress;
    }

    private void OnDisable()
    {
        GameEvents.onOrderAccepted -= ShowOrderProgress;
    }

    public void ShowTakeOrder()
    {
        takeOrder.SetActive(true);
        orderComplete.SetActive(false);
        questProgress.SetActive(false);
    }

    public void ShowOrderComplete()
    {
        StartCoroutine(ShowOrderCompleteCoroutine());
    }

    IEnumerator ShowOrderCompleteCoroutine()
    {
        takeOrder.SetActive(false);
        orderComplete.SetActive(true);
        questProgress.SetActive(false);

        RectTransform orderCompleteRect = orderComplete.GetComponent<RectTransform>();
        float origY = orderCompleteRect.anchoredPosition.y;

        orderCompleteRect.DOAnchorPosY(275.5f, .25f);
        yield return new WaitForSeconds(1.5f);
        takeOrder.SetActive(true);
        orderCompleteRect.DOAnchorPosY(origY, .25f).OnComplete(ShowTakeOrder);
    }

    public void ShowOrderProgress(Quest q, float delay)
    {
        trackedQuest = q;
        takeOrder.SetActive(false);
        orderComplete.SetActive(false);
        questProgress.SetActive(true);

        questProgressText.text = 0 + "/" + trackedQuest.amountToDeliver + " " + trackedQuest.targetPotion.potionName + " potions";
    }

    public void UpdateQuestProgressText(int amount)
    {
        questProgressText.text = amount + "/" + trackedQuest.amountToDeliver + " " + trackedQuest.targetPotion.potionName + " potions";
    }

    public void PlayQuestCompleteParticles()
    {
        questCompleteParticles.Play();
    }

}
