using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private AudioSource AudioSource;

    public float FadeDuration = 1.0f;

    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float TargetVolume = AudioSource.volume;
        AudioSource.volume = 0f;
        AudioSource.Play();

        float ElapsedTime = 0f;

        while(ElapsedTime < FadeDuration)
        {
            ElapsedTime += Time.deltaTime;
            AudioSource.volume = Mathf.Lerp(0f, TargetVolume, ElapsedTime / FadeDuration);
            yield return null;
        }

        AudioSource.volume = TargetVolume;
    }
}
