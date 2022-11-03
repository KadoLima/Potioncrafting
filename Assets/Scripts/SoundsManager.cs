using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SFXSound
{
    public AudioClip audioClip;
    [Range(0.0f, 1.0f)] public float audioVolume;
}

public class SoundsManager : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField] AudioSource sfxAudioSource;
    [SerializeField] SFXSound orderPanelToggleSound;
    [SerializeField] SFXSound orderAcceptedSound;
    [SerializeField] SFXSound movePlatformSound;
    [SerializeField] SFXSound openPlatformSound;
    //[SerializeField] SFXSound spawnedLiquidSound;
    [SerializeField] SFXSound flaskFullSound;
    [SerializeField] SFXSound finishingSound;
    [SerializeField] SFXSound mixingSound;
    [SerializeField] SFXSound recipeBookSound;
    [SerializeField] SFXSound questCompleteSound;


    public static SoundsManager instance;

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

    }

    private void OnDisable()
    {

    }



    public void Play_OrderPanelToggleSound() => sfxAudioSource.PlayOneShot(orderPanelToggleSound.audioClip,orderPanelToggleSound.audioVolume);
    public void Play_OrderAcceptedSound() => sfxAudioSource.PlayOneShot(orderAcceptedSound.audioClip,orderAcceptedSound.audioVolume);
    public void Play_MovePlatformSound() => sfxAudioSource.PlayOneShot(movePlatformSound.audioClip,movePlatformSound.audioVolume);
    public void Play_OpenPlatformSound() => sfxAudioSource.PlayOneShot(openPlatformSound.audioClip, openPlatformSound.audioVolume);
    public void Play_FlaskFullSound() => sfxAudioSource.PlayOneShot(flaskFullSound.audioClip,flaskFullSound.audioVolume);
    public void Play_FinishingSound() => sfxAudioSource.PlayOneShot(finishingSound.audioClip, finishingSound.audioVolume);
    public void Play_MixingSound() => sfxAudioSource.PlayOneShot(mixingSound.audioClip,mixingSound.audioVolume);
    public void Play_RecipeBookSound() => sfxAudioSource.PlayOneShot(recipeBookSound.audioClip,recipeBookSound.audioVolume);
    public void Play_QuestCompletedSound() => sfxAudioSource.PlayOneShot(questCompleteSound.audioClip, questCompleteSound.audioVolume);
   //public void Play_SpawnedLiquid() => sfxAudioSource.PlayOneShot(spawnedLiquidSound.audioClip, spawnedLiquidSound.audioVolume);
}
