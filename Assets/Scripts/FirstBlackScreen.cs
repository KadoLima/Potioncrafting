using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FirstBlackScreen : MonoBehaviour
{
    Image image;
    [SerializeField] GameObject tutorialScreen;

    private void Awake()
    {
        image = GetComponent<Image>();

        tutorialScreen.SetActive(false);

    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    IEnumerator FadeOutCoroutine()
    {
        image.color = new Color(0, 0, 0, 1);
        yield return new WaitForSeconds(.5f);
        image.DOColor(new Color(0, 0, 0, .5f), 1f).OnComplete(ShowTutorialScreen);
    }

    void ShowTutorialScreen()
    {
        tutorialScreen.transform.localScale = Vector3.zero;
        tutorialScreen.SetActive(true);
        tutorialScreen.transform.DOScale(Vector3.one, .25f).SetEase(Ease.OutBack);
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }
}
