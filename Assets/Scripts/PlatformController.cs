using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;



public class PlatformController : MonoBehaviour
{
    public enum MovingStates
    {
        ROTATING_LEFT,
        ROTATING_RIGHT,
        OPENMID,
        IDLE
    }

    MovingStates currentMovingState;
    [SerializeField] Color platformColor;
    [SerializeField]float maxRotationDeg = 20;
    float timeToMaxRotate = .25f;
    float timeToOpenMid = .25f;
    [SerializeField] Transform leftPlatform;
    [SerializeField] Transform rightPlatform;
    [SerializeField] SpriteRenderer junction;

    float defaultScale;

    // Start is called before the first frame update
    void Start()
    {
        currentMovingState = MovingStates.IDLE;
        ShowJunction();

        defaultScale = leftPlatform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlatform();
        PlaySounds();
    }

    private void MovePlatform()
    {
        if (currentMovingState!= MovingStates.OPENMID)
        {
            if (Input.GetKey(KeyCode.D))
            {
                RotateRight();
            }

            else if (Input.GetKey(KeyCode.A))
            {
                RotateLeft();
            }
        }

        if (Input.GetKey(KeyCode.C) || Input.GetKey(KeyCode.Space))
        {
            if (currentMovingState != MovingStates.ROTATING_LEFT && currentMovingState != MovingStates.ROTATING_RIGHT)
                OpenMid();
        }


        if (Input.GetKeyUp(KeyCode.D) && currentMovingState != MovingStates.ROTATING_LEFT || Input.GetKeyUp(KeyCode.A) && currentMovingState != MovingStates.ROTATING_RIGHT)
        {
            ResetRotation();
        }

        else if (Input.GetKeyUp(KeyCode.C) || Input.GetKeyUp(KeyCode.Space))
        {
            ResetMid();
        }
    }

    void RotateLeft()
    {
        transform.DORotate(new Vector3(0, 0, maxRotationDeg), timeToMaxRotate);
        currentMovingState = MovingStates.ROTATING_LEFT;
    }

    void RotateRight()
    {
        transform.DORotate(new Vector3(0, 0, -maxRotationDeg), timeToMaxRotate);
        currentMovingState = MovingStates.ROTATING_RIGHT;
    }
    
    void ResetRotation()
    {
        transform.DORotate(Vector3.zero, timeToMaxRotate);
        currentMovingState = MovingStates.IDLE;
    }

    float OpeningAngle(string platformSide)
    {
        if (platformSide.ToLower() == "left")
            return -maxRotationDeg - 15;

        return maxRotationDeg + 15;
    }

    float OpeningScale()
    {
        return defaultScale - .5f;
    }

    private void OpenMid()
    {
        leftPlatform.DOLocalRotate(new Vector3(0, 0, OpeningAngle("left")), timeToMaxRotate);
        leftPlatform.DOScaleX(OpeningScale(), timeToMaxRotate);
        rightPlatform.DOLocalRotate(new Vector3(0, 0, OpeningAngle("right")), timeToMaxRotate);
        rightPlatform.DOScaleX(OpeningScale(), timeToMaxRotate);
        HideJunction();
        currentMovingState = MovingStates.OPENMID;
    }

    void ResetMid()
    {
        leftPlatform.DOLocalRotate(Vector3.zero, timeToMaxRotate);
        leftPlatform.DOScaleX(defaultScale, timeToMaxRotate);
        rightPlatform.DOLocalRotate(Vector3.zero, timeToMaxRotate);
        rightPlatform.DOScaleX(defaultScale, timeToMaxRotate);
        StartCoroutine(ShowJunction_Coroutine(timeToMaxRotate));
        currentMovingState = MovingStates.IDLE;
    }

    void ShowJunction()
    {
        junction.DOColor(new Color(platformColor.r, platformColor.g, platformColor.b, 1), .1f);
    }

    IEnumerator ShowJunction_Coroutine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime-.1f);
        ShowJunction();
    }

    void HideJunction()
    {
        junction.DOColor(new Color(platformColor.r, platformColor.g, platformColor.b, 0), .01f);
    }

    void PlaySounds()
    {
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            SoundsManager.instance.Play_MovePlatformSound();
        }

        if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.Space))
        {
            SoundsManager.instance.Play_OpenPlatformSound();
        }
    }
}
