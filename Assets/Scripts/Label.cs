using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Label : MonoBehaviour
{
    [SerializeField] SpriteRenderer labelColor;
    ColorClass currentColorClass;

    // Start is called before the first frame update
    void Start()
    {
        //
    }

    public void SetToGray()
    {
        labelColor.color = Color.gray;
    }

    public void StartColor(Color c)
    {
        labelColor.color = c;
    }

    public void SetColor(ColorClass c)
    {
        currentColorClass = c;
        labelColor.color = c.color;
    }

    public ColorClass GetCurrentColorClass()
    {
        return currentColorClass;
    }


}
