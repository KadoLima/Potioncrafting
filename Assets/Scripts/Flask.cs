using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using TMPro;
using System.Linq;

public class Flask : MonoBehaviour
{
    [SerializeField] int flaskID;
    public int FlaskID => flaskID;

    [SerializeField] LiquidColor startColor;
    public LiquidColor StartColor => startColor;

    [SerializeField] FinishButton button;
    [SerializeField] GameObject cork;
    [SerializeField] GameObject colliders;
    [SerializeField] SpriteRenderer halfMark;
    [SerializeField] TrailRenderer trail;

    ColorClass currentColor;
    float startY;
    float corkStartY;
    [SerializeField] float limit = 90f;
    public float Limit => limit;
    public List<GameObject> contentList = new List<GameObject>();
    bool isReady;
    public bool IsReady => isReady;

    LiquidColor mainFilledColor, secondFilledColor;
    MixClass resultMix;

    Dictionary<LiquidColor, float> liquidParticlesCount = new Dictionary<LiquidColor, float>();
    public Dictionary<LiquidColor, float> LiquidParticlesCount
    {
        get => liquidParticlesCount;
        set => liquidParticlesCount = value;
    }

    public ColorClass CurrentColorClass
    {
        get => currentColor;
        set
        {
            currentColor = value;
            //currentColorType = currentColor.colorType;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        startY = this.transform.localPosition.y;
        corkStartY = cork.transform.localPosition.y;

        //button.onClick.AddListener(DeliverFlask);

        cork.SetActive(false);

        SetStartingColor();

        button.gameObject.SetActive(false);

        isReady = true;

        liquidParticlesCount.Add(LiquidColor.RED, 0);
        liquidParticlesCount.Add(LiquidColor.YELLOW, 0);
        liquidParticlesCount.Add(LiquidColor.BLUE, 0);

        SetTrail(false);
    }


    void SetStartingColor()
    {
        CurrentColorClass = ColorsManager.instance.GetClassByLiquidColor(startColor);
    }


    public void DeliverFlask(bool needMixing)
    {
        StartCoroutine(DeliverFlaskCoroutine(needMixing));
    }

    IEnumerator DeliverFlaskCoroutine(bool needMixing)
    {
        //Debug.LogWarning("DELIVERING FLASK...");

        float timeToHide = .25f;
        float waitTime = .25f;

        DisableLiquidComponents();
        button.gameObject.SetActive(false);

        if (needMixing)
        {
            float shakeTime = .5f;
            ShakeFlask();
            resultMix = ColorsManager.instance.FindMixClass(mainFilledColor, secondFilledColor);
            Debug.LogWarning(resultMix.mixColor);
            ChangeContentColor(resultMix);
            yield return new WaitForSeconds(shakeTime+.25f);
        }

        this.transform.DOLocalMoveY(-8, timeToHide).SetEase(Ease.InQuint);
        SetTrail(true);
        StartCoroutine(EarnMoneyCoroutine(needMixing, CurrentColorClass.liquidColor));
        QuestsManager.instance.AddToCurrentQuest(mainFilledColor,secondFilledColor,resultMix);
        SoundsManager.instance.Play_FinishingSound();

        yield return new WaitForSeconds(timeToHide + waitTime);

        HideCork();
        ResetFlask();
        colliders.SetActive(false);
        this.transform.DOLocalMoveY(startY, timeToHide).SetEase(Ease.InQuint);

        yield return new WaitForSeconds(.1f);

        isReady = true;
        yield return new WaitForSeconds(1.5f);
        colliders.SetActive(true);
    }

    IEnumerator EarnMoneyCoroutine(bool isMixture, LiquidColor liquid)
    {
        yield return new WaitForSeconds(0.25f);
        float moneyToEarn = ColorsManager.instance.PotionValue(isMixture ? resultMix.baseColor : currentColor.color);
        Debug.LogWarning("MONEY TO EARN: " + moneyToEarn);
        GameEvents.FinishFlask(Mathf.CeilToInt(moneyToEarn), liquid, this.flaskID);
    }

    private void ResetFlask()
    {
        foreach (GameObject c in contentList)
        {
            Destroy(c);
        }

        contentList.Clear();

        for (int i = 0; i < ColorsManager.instance.AllColors.Length; i++)
        {
            if (LiquidParticlesCount.ContainsKey(ColorsManager.instance.AllColors[i].liquidColor))
                LiquidParticlesCount[ColorsManager.instance.AllColors[i].liquidColor] = 0;
        }

        ColorHalfMark(false);

        mainFilledColor = secondFilledColor = LiquidColor.NULL;
        resultMix = null;

        SetTrail(false);
    }

    public void CheckIfCompleted()
    {
        if (IsFull() && isReady)
        {
            CloseFlask();
        }
    }

    void CloseFlask()
    {
        CheckPurity();
        isReady = false;
        ScaleCork();

        SoundsManager.instance.Play_FlaskFullSound();

    }

    void CheckPurity()
    {
        //Debug.LogWarning("CHECKING FLASK CONTENT...");

        foreach (var c in liquidParticlesCount)
        {
            //Debug.LogWarning(c.Key + ": " + c.Value);
            float c_corrected = c.Value / limit;

            if (c_corrected >= FlasksManager.instance.OneColorThreshold)
            {
                {
                    //Debug.LogWarning("ONE COLOR POTION!");
                    button.gameObject.SetActive(true);
                    button.ShowFinishInner();
                    mainFilledColor = c.Key;
                    secondFilledColor = LiquidColor.NULL;
                    return;
                }
            }
        }

        mainFilledColor = LiquidParticlesCount.OrderByDescending(x => x.Value).First().Key;
        secondFilledColor = LiquidParticlesCount.OrderByDescending(x => x.Value).Skip(1).First().Key;

        //Debug.LogWarning($"Main color is: {mainColor} / Second color is: {secondColor}");

        button.gameObject.SetActive(true);
        button.ShowMixInner();
    }

    private void ScaleCork()
    {
        cork.transform.localScale = Vector2.zero;
        cork.SetActive(true);
        cork.transform.DOScale(new Vector2(1.1f, 0.7f), .15f).SetEase(Ease.InOutQuint).OnComplete(PlaceCork);
    }

    void PlaceCork()
    {
        cork.transform.DOLocalMoveY(-0.61f, .25f).SetEase(Ease.OutQuint);
    }

    void HideCork()
    {
        cork.transform.DOLocalMoveY(corkStartY, 0f);
        cork.SetActive(false);
    }

    void SetTrail(bool s)
    {
        trail.emitting = s;
    }

    void ShakeFlask()
    {
        SoundsManager.instance.Play_MixingSound();
        this.transform.DOLocalRotate(new Vector3(transform.localRotation.x, transform.localRotation.y, -5), .025f).OnComplete(RotateFlaskAnim);
    }

    private void RotateFlaskAnim()
    {
        this.transform.DOLocalRotate(new Vector3(transform.localRotation.x, transform.localRotation.y, 5), .06f).SetLoops(8, LoopType.Yoyo).SetEase(Ease.InOutQuad).OnComplete(ResetRotation);
    }

    void ResetRotation()
    {
        this.transform.DOLocalRotate(Vector3.zero, .1f);
    }

    public bool IsFull()
    {
        for (int i = 0; i < contentList.Count; i++)
        {
            if (contentList[i] == null)
                contentList.Remove(contentList[i]);
        }

        return contentList.Count >= limit;
    }



    void DisableLiquidComponents()
    {
        foreach (GameObject g in contentList)
        {
            if (g != null)
            {
                Destroy(g.GetComponent<Rigidbody2D>());
                g.GetComponent<LiquidParticle>().DisableTrail();
            }
        }
    }

    public bool IsHalfFullOfIndicatedLiquid()
    {
        float x = liquidParticlesCount[startColor];

        if (x >= (limit / 2))
        {
            ColorHalfMark(true);
            return true;
        }
        ColorHalfMark(false);
        return false;
    }

    public void ColorHalfMark(bool state)
    {
        if (state)
        {
            halfMark.color = Color.green;
        }
        else halfMark.color = new Color(Color.white.r, Color.white.g, Color.white.b, 0.33f);
    }

    void ChangeContentColor(MixClass mix)
    {
        foreach (GameObject liquidItem in contentList)
        {
            if (liquidItem != null)
            {
                Material tempMaterial = new Material(liquidItem.GetComponent<SpriteRenderer>().sharedMaterial);
                liquidItem.GetComponent<SpriteRenderer>().sharedMaterial = tempMaterial;

                StartCoroutine(LerpColorCoroutine(tempMaterial, "_highlightColor", tempMaterial.GetColor("_highlightColor"), mix.highlightColor, 0.3f));
                StartCoroutine(LerpColorCoroutine(tempMaterial, "_baseColor", tempMaterial.GetColor("_baseColor"), mix.baseColor, 0.3f));
            }
                
        }

    }

    IEnumerator LerpColorCoroutine(Material mat, string propertyName, Color initialColor, Color finalColor, float lerpDuration)
    {
        float percentage = 0;
        float startTime = Time.time;
        Color currentColor;

        while (percentage < 1f)
        {
            percentage = (Time.time - startTime) / lerpDuration;
            currentColor = Color.Lerp(initialColor, finalColor, percentage);
            mat.SetColor(propertyName, currentColor);
            yield return null;
        }

    }


}
