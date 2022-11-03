using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlasksManager : MonoBehaviour
{
    [SerializeField] Flask[] flasks;

    public static FlasksManager instance;

    [Header("Rules")]
    [SerializeField] float oneColorThreshold = .7f;
    public float OneColorThreshold => oneColorThreshold;

    [SerializeField] float twoColorThreshold = .45f;
    public float TwoColorThreshold => twoColorThreshold;

    int flasksCreated = 0;
    public int FlasksCreated => flasksCreated;

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
        GameEvents.onFinishFlask += AddToFlasksCreatedCount;
    }


    public bool FlasksReady()
    {
        foreach (Flask f in flasks)
        {
            if (!f.IsReady)
                return false;
        }

        return true;
    }

    public void AddToFlasksCreatedCount(int a, LiquidColor lc, int b)
    {
        flasksCreated++;
    }


}
