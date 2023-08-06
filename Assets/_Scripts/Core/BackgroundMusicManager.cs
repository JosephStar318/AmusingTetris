using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusicManager : MonoBehaviour
{
    private AudioSource audioSource;

    private IEnumerator VolumeDownMusic(float target, float duration)
    {
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            audioSource.volume = Mathf.Lerp(1, target, elapsedTime / duration);
            elapsedTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
    private IEnumerator FadeOutMusic(float duration)
    {
        float elapsedTime = 1 - audioSource.volume;

        while(elapsedTime < duration)
        {
            audioSource.volume = Mathf.Lerp(1, 0, elapsedTime / duration);
            elapsedTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        audioSource.volume = 0;
    }
}
