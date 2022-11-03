using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PotionInfo
{
    public Sprite potionSprite;
    public LiquidColor liquidColor;
    public MixColor mixColor;
    public string potionName;
    public string recipeDesc;
    public float sellPrice;
    public bool isLearned;
}
public class PotionsManager : MonoBehaviour
{
    [SerializeField] PotionInfo[] allPotions;
    public PotionInfo[] AllPotions => allPotions;

    [SerializeField] GameObject potionRecipeItem;
    [SerializeField] GameObject recipesScreen;
    [SerializeField] Transform potionItensParent;

    public static PotionsManager instance;

    private void Awake()
    {
        instance = this;

        Time.timeScale = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        ShowHideRecipesScreen(false);

        BuildRecipeBook();
    }

    public List<PotionInfo> AllPurePotions()
    {
        List<PotionInfo> allPurePotions = new List<PotionInfo>();

        foreach (var item in AllPotions)
        {
            if (item.mixColor == MixColor.NULL)
                allPurePotions.Add(item);
        }

        return allPurePotions;
    }

    public List<PotionInfo> AllMixPotions()
    {
        List<PotionInfo> allMixPotions = new List<PotionInfo>();

        foreach (var item in AllPotions)
        {
            if (item.mixColor != MixColor.NULL)
                allMixPotions.Add(item);
        }

        return allMixPotions;
    }

    void BuildRecipeBook()
    {
        foreach (PotionInfo p in allPotions)
        {
            GameObject go = Instantiate(potionRecipeItem, potionItensParent);
            go.GetComponent<PotionRecipeItem>().Build(p.potionSprite, 
                                                      p.potionName, 
                                                      p.recipeDesc, 
                                                      p.sellPrice.ToString());

            go.SetActive(true);
        }
    }

    public void ShowHideRecipesScreen(bool state)
    {
        recipesScreen.SetActive(state);

        if (state)
        {
            Time.timeScale = 0;
            SoundsManager.instance.Play_RecipeBookSound();
        }
        else Time.timeScale = 1;
    }

}
