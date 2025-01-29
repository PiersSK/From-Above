using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    public AudioSource genericSFXSource;
    public AudioSource bgMusicSource;

    public AudioMixerGroup bgMixer;
    private float bgVol = 0.3f;
    private List<AudioSource> fadingSources = new();

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

        if(!fadingSources.Contains(bgMusicSource)) AutoAdjustBGForOtherTracks();
    }

    public void PlaySFXOneShot(AudioClip clip, float maxPitchVariation = 0f, float volume = 0.3f, float maxVolumeVariation = 0f)
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

    private void AutoAdjustBGForOtherTracks()
    {
        int musicSourcesPlaying = 0;

        foreach (var audioSource in FindObjectsByType<AudioSource>(FindObjectsSortMode.None))
        {
            if (audioSource.outputAudioMixerGroup == bgMixer && audioSource.isPlaying && audioSource != bgMusicSource)
            {
                Debug.Log(audioSource.name);
                musicSourcesPlaying++;
            }
        }

        bgMusicSource.volume = musicSourcesPlaying > 0 ? bgVol * 0.25f : bgVol;
    }

    public IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        fadingSources.Add(audioSource);
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Pause();
        audioSource.volume = startVolume;
        fadingSources.Remove(audioSource);
    }

    public IEnumerator FadeIn(AudioSource audioSource, float targetVol, float FadeTime)
    {
        fadingSources.Add(audioSource);
        audioSource.volume = 0f;
        audioSource.Play();

        while (audioSource.volume < targetVol)
        {
            audioSource.volume += targetVol * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.volume = targetVol;
        fadingSources.Remove(audioSource);
    }
}
