using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LiquidColor
{
    BLUE,
    YELLOW,
    RED,
    NULL
}

public enum MixColor
{
    GREEN, 
    ORANGE, 
    PURPLE,
    NULL
}

[System.Serializable]
public class ColorClass
{
    public LiquidColor liquidColor;
    public Color color;
    public LiquidColor mixLiquidColor1;
    public LiquidColor mixLiquidColor2;
    //public bool needsMixing = false;
    public GameObject prefab;
    public float priceToSell;
}

[System.Serializable]
public class MixClass
{
    public MixColor mixColor;
    public Color baseColor;
    public Color highlightColor;
    public LiquidColor color1;
    public LiquidColor color2;
    public float priceToSell;
}

public class ColorsManager : MonoBehaviour
{
    [SerializeField] ColorClass[] allColors;
    public ColorClass[] AllColors => allColors;

    [SerializeField] MixClass[] allMixtures;
    public MixClass[] AllMixtures => allMixtures;

    public static ColorsManager instance;

    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
    }

    public ColorClass GetClassByLiquidColor(LiquidColor c)
    {
        foreach (var item in allColors)
        {
            if (item.liquidColor == c)
                return item;
        }
        return null;
    }

    public ColorClass ChooseRandomColor()
    {
        int rnd = Random.Range(0, allColors.Length);
        return allColors[rnd];
    }

    public MixClass FindMixClass(LiquidColor c1, LiquidColor c2)
    {
        foreach (var item in AllMixtures)
        {
            if ((item.color1 == c1 && item.color2 == c2) ||
                (item.color1 == c2 && item.color2 == c1))
            {
                Debug.LogWarning("MIX COLOR IS: " + item.mixColor);
                return item;
            }
        }

        return null;
    }

    public float PotionValue(Color c)
    {
        foreach (var item in allColors)
        {
            if (item.color == c)
            {
                return item.priceToSell;
            }
        }

        foreach (var item in AllMixtures)
        {
            if (item.baseColor == c)
            {
                return item.priceToSell;
            }
        }

        Debug.LogWarning("CANT FIND PRICE");
        return -1;
    }
}
