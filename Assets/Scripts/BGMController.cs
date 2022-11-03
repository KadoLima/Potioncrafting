using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    [Header("Background Music")]
    [SerializeField] AudioSource bgmAudioSource;
    [SerializeField] SFXSound backgroundMusic1;
    [SerializeField] SFXSound backgroundMusic2;
    // Start is called before the first frame update
    void Start()
    {
        PlayBGMLoop();
    }

    void PlayBGMLoop(float delay = 0)
    {
        StartCoroutine(PlayBGMLoopCoroutine(delay));
    }

    IEnumerator PlayBGMLoopCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        while (true)
        {
            AudioClip chosenClip = null;
            float chosenVolume = 0f;
            if (Random.value <= .5f)
            {
                chosenClip = backgroundMusic1.audioClip;
                chosenVolume = backgroundMusic1.audioVolume;
            }
            else
            {
                chosenClip = backgroundMusic2.audioClip;
                chosenVolume = backgroundMusic2.audioVolume;
            }
            bgmAudioSource.PlayOneShot(chosenClip,chosenVolume);
            yield return new WaitForSeconds(chosenClip.length - 1f);
            bgmAudioSource.Stop();
        }
    }
}
