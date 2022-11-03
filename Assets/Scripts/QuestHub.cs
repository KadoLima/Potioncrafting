using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class QuestHub : MonoBehaviour
{
    [SerializeField] HorizontalLayoutGroup horizontalLayout;
    [SerializeField] float startSpacing;
    [SerializeField] float finalSpacing;
    [SerializeField] GameObject questCardPrefab;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnEnable()
    {
        CreateQuests();
        ShowOnScreen();

        GameEvents.onOrderAccepted += HideQuestsHub;
    }

    private void OnDisable()
    {
        GameEvents.onOrderAccepted -= HideQuestsHub;
    }

    public void InitialAnimation()
    {
        horizontalLayout.spacing = startSpacing;
        float currentSpacing = startSpacing;
        DOTween.To(() => currentSpacing, x => currentSpacing = x, finalSpacing, .3f).SetEase(Ease.OutExpo).OnUpdate(() => { horizontalLayout.spacing = currentSpacing; }).OnComplete(FinalBounceEffect);
    }

    IEnumerator InitialAnimationCoroutine()
    {
        horizontalLayout.spacing = startSpacing;
        yield return new WaitForSeconds(.05f);
        float currentSpacing = startSpacing;
        DOTween.To(() => currentSpacing, x => currentSpacing = x, finalSpacing, .3f).SetEase(Ease.InOutExpo).OnUpdate(() => { horizontalLayout.spacing = currentSpacing; }).OnComplete(FinalBounceEffect);
    }

    void FinalBounceEffect()
    {
        float currentSpacing = finalSpacing;
        DOTween.To(() => currentSpacing, x => currentSpacing = x, finalSpacing - 15, .1f).OnUpdate(() => { horizontalLayout.spacing = currentSpacing; });
    }

    public void ShowOnScreen()
    {
        gameObject.SetActive(true);
        RectTransform questsHubRect = gameObject.GetComponent<RectTransform>();
        questsHubRect.DOAnchorPos(Vector2.zero, .35f).SetEase(Ease.OutQuad).OnComplete(InitialAnimation);

        SoundsManager.instance.Play_OrderPanelToggleSound();
    }

    public void HideQuestsHub()
    {
        StartCoroutine(HideHubCoroutine(0));
    }


    public void HideQuestsHub(Quest q,float delay)
    {
        StartCoroutine(HideHubCoroutine(delay));
    }

    IEnumerator HideHubCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        RectTransform questsHubRect = gameObject.GetComponent<RectTransform>();
        questsHubRect.DOAnchorPos(new Vector2(0, 1000f), .5f).SetEase(Ease.OutExpo);
        yield return new WaitForSeconds(.4f);
        horizontalLayout.spacing = startSpacing;
        gameObject.SetActive(false);
    }

    public int QuestSpotsAvailable()
    {
        int cardsCreated = 0;

        for (int i = 0; i < horizontalLayout.transform.childCount; i++)
        {
            if (horizontalLayout.transform.GetChild(i).gameObject.activeSelf)
                cardsCreated++;
        }

        return 3 - cardsCreated;
    }

    public void CreateQuestCard(Quest q)
    {
        GameObject g = Instantiate(questCardPrefab, horizontalLayout.transform);
        //Debug.LogWarning(q);
        g.GetComponent<QuestCard>().Build(q);
        g.SetActive(true);
    }

    public void CreateQuests()
    {
        int spotsAvailable = QuestSpotsAvailable();
        for (int i = 0; i < spotsAvailable; i++)
        {
            float chance = Random.value;

            if (chance > 0.5f)//regular potion
            {
                int amount = UnityEngine.Random.Range(3, 6);

                string randomQuestDesc = QuestsManager.instance.QuestsDescriptionsPool[Random.Range(0, QuestsManager.instance.QuestsDescriptionsPool.Length)];
                PotionInfo randomPurePotion = PotionsManager.instance.AllPurePotions()[UnityEngine.Random.Range(0, PotionsManager.instance.AllPurePotions().Count)];
                Quest q = new Quest(randomQuestDesc, randomPurePotion, amount);
                CreateQuestCard(q);
            }
            else //mixture
            {
                int amount = UnityEngine.Random.Range(2, 5);
                string randomQuestDesc = QuestsManager.instance.QuestsDescriptionsPool[Random.Range(0, QuestsManager.instance.QuestsDescriptionsPool.Length)];
                PotionInfo randomMixPotion = PotionsManager.instance.AllMixPotions()[UnityEngine.Random.Range(0, PotionsManager.instance.AllMixPotions().Count)];
                Quest q = new Quest(randomQuestDesc, randomMixPotion, amount);
                CreateQuestCard(q);
                //Debug.LogWarning("CREATED MIXTURE QUEST");
            }
        }
    }
}
