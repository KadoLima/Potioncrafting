using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestCard : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI descriptionText;
    [SerializeField] TextMeshProUGUI objectiveText;
    [SerializeField] Image potionImage;
    [SerializeField] TextMeshProUGUI rewardValueText;
    Quest currentQuest = null;
    [SerializeField] ParticleSystem particles;
    [SerializeField] GameObject blocker;
    [SerializeField] Button acceptButton;
    [SerializeField] TextMeshProUGUI buttonText;
    bool destroyWhenInvisible = false;

    float[] multipliers;


    // Start is called before the first frame update
    void Awake()
    {
        SetMultipliers();

        blocker.SetActive(false);
    }

    private void OnEnable()
    {
        destroyWhenInvisible = false;
        EnableAcceptButton();
    }

    private void EnableAcceptButton()
    {
        acceptButton.interactable = true;
        buttonText.text = "ACCEPT";
    }

    private void DisableAcceptButton()
    {
        acceptButton.interactable = false;
        buttonText.text = "ACCEPTED!";
    }

    private void OnDisable()
    {
        blocker.SetActive(false);

        if (destroyWhenInvisible)
            Destroy(gameObject);
    }

    private void SetMultipliers()
    {
        multipliers = new float[5];
        multipliers[0] = 1.1f;
        multipliers[1] = 1.15f;
        multipliers[2] = 1.2f;
        multipliers[3] = 1.25f;
        multipliers[4] = 1.3f;
    }

    public void Build(Quest q)
    {
        currentQuest = q;
        descriptionText.text = q.description;
        objectiveText.text = "<size=80>" + q.amountToDeliver + "</size>" + " " + q.targetPotion.potionName + " potions";
        potionImage.sprite = q.targetPotion.potionSprite;
        rewardValueText.text = "REWARD: " + "<color=#008000ff>$" + (q.targetPotion.sellPrice * q.amountToDeliver * multipliers[Random.Range(0,multipliers.Length)]).ToString("F0")+"</color>";
    }

    public void AcceptQuest()
    {
        particles.Play();
        HighlightThisCard();
        DisableAcceptButton();
        destroyWhenInvisible = true;

        GameEvents.AcceptOrder(currentQuest,.5f);

        SoundsManager.instance.Play_OrderAcceptedSound();
    }

    private void HighlightThisCard()
    {
        Transform parent = this.transform.parent;

        foreach (Transform child in parent)
        {
            if (child.gameObject != this.gameObject && child.GetComponent<QuestCard>())
                child.GetComponent<QuestCard>().blocker.SetActive(true);
        }
    }
}
