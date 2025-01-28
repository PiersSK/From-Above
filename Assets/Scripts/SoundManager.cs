using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    public AudioSource genericSFXSource;
    public AudioSource bgMusicSource;
    private float bgVol = 0.3f;
    public string clipPlaying = string.Empty;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        bgVol = bgMusicSource.volume;
    }

    private void Update()
    {
        if (!genericSFXSource.isPlaying) clipPlaying = string.Empty;
    }

    public void PlaySFXOneShot(AudioClip clip, float maxPitchVariation = 0f, float volume = 1f, float maxVolumeVariation = 0f)
    {
        float vMod = maxVolumeVariation == 0f ? 0f : Random.Range(-maxVolumeVariation, maxVolumeVariation);
        float pMod = maxPitchVariation == 0f ? 0f : Random.Range(-maxPitchVariation, maxPitchVariation);

        genericSFXSource.pitch = 1 + pMod;
        genericSFXSource.PlayOneShot(clip, volume + vMod);
        clipPlaying = clip.name;
    }

    public void FadeOutBgMusic(float fadeTime)
    {
        StartCoroutine(FadeOut(bgMusicSource, fadeTime));
    }

    public void FadeInBgMusic(float fadeTime)
    {
        StartCoroutine(FadeIn(bgMusicSource, bgVol, fadeTime));
    }

    public IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Pause();
        audioSource.volume = startVolume;
    }

    public IEnumerator FadeIn(AudioSource audioSource, float targetVol, float FadeTime)
    {
        audioSource.volume = 0f;
        audioSource.Play();

        while (audioSource.volume < targetVol)
        {
            audioSource.volume += targetVol * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.volume = targetVol;
    }
}
