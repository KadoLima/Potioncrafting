using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveEffect : MonoBehaviour
{
    [SerializeField] Material material;

    bool isDissolving;
    float dissolveAmount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDissolving)
        {
            dissolveAmount = Mathf.Clamp01(dissolveAmount + Time.deltaTime);
            material.SetFloat("_dissolveAmount", dissolveAmount);
        }
        else
        {
            dissolveAmount = Mathf.Clamp01(dissolveAmount - Time.deltaTime);
            material.SetFloat("_dissolveAmount", dissolveAmount);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            isDissolving = true;
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            isDissolving = false;
        }
    }
}
