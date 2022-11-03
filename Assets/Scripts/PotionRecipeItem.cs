using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PotionRecipeItem : MonoBehaviour
{
    [SerializeField] Image potionSprite;
    [SerializeField] TextMeshProUGUI potionNameText;
    [SerializeField] TextMeshProUGUI recipeText;
    [SerializeField] TextMeshProUGUI valueText;

    private void OnEnable()
    {
        SetBackgroundAlpha();
    }

    private void SetBackgroundAlpha()
    {
        float alpha;
        if (this.transform.GetSiblingIndex() % 2 == 0)
        {
            alpha = .1f;
        }
        else alpha = 0f;

        GetComponent<Image>().color = new Color(0, 0, 0, alpha);
    }

    public void Build(Sprite sprite, string potionNameText, string recipeText, string valueText)
    {
        this.potionSprite.sprite = sprite;
        this.potionNameText.text = potionNameText + " Potion";
        this.recipeText.text = recipeText;
        this.valueText.text = valueText;
    }
}
