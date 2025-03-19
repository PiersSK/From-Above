using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    public AudioSource genericSFXSource;
    public AudioSource bgMusicSource;
    public AudioSource shipPASource;

    public AudioMixerGroup bgMixer;
    private float bgVol = 0.3f;
    
    public List<AudioSource> pausedSources = new();
    private List<AudioSource> fadingSources = new();

    public string clipPlaying = string.Empty;

    public bool bgPaused = false;

    private string incrementSoundtrack = "IncrementSoundtrack";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
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

    public void PlaySFXOneShotSetPitchAndVolume(AudioClip clip, float pitch = 1f, float volume = 0.3f)
    {

        genericSFXSource.pitch = pitch;
        genericSFXSource.PlayOneShot(clip, volume);
        clipPlaying = clip.name;
    }

    public void PlayShipPALine(AudioClip clip, float pitch = 1f, float volume = 0.3f)
    {
        shipPASource.clip = clip;
        shipPASource.pitch = pitch;
        shipPASource.volume = volume;
        shipPASource.Play();
    }

    public void PauseBgMusic(float fadeTime)
    {
        if (bgMusicSource.isPlaying)
        {
            bgPaused = true;
            bgMusicSource.Pause();
        }
    }

    public void RestartBgMusic(float fadeTime)
    {
        if (!bgMusicSource.isPlaying && bgPaused)
        {
            bgMusicSource.Play();
            bgPaused = false;
        }
    }

    public void PauseAllSound()
    {
        foreach (var audioSource in FindObjectsByType<AudioSource>(FindObjectsSortMode.None))
        {
            if (audioSource.isPlaying) {
                audioSource.Pause();
                pausedSources.Add(audioSource);
                if (audioSource.outputAudioMixerGroup == bgMixer) bgPaused = true;
            }
        }
    }

    public void UnpauseAllPausedSound()
    {
        foreach (var audioSource in pausedSources)
        {
            audioSource.UnPause();
            if (audioSource.outputAudioMixerGroup == bgMixer) bgPaused = false;
        }

        pausedSources.Clear();
    }

    private void AutoAdjustBGForOtherTracks()
    {
        int musicSourcesPlaying = 0;

        foreach (var audioSource in FindObjectsByType<AudioSource>(FindObjectsSortMode.None))
        {
            if (audioSource.outputAudioMixerGroup == bgMixer && audioSource.isPlaying && audioSource != bgMusicSource)
            {
                musicSourcesPlaying++;
            }
        }

        bgMusicSource.volume = musicSourcesPlaying > 0 ? bgVol * 0.6f : bgVol;
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
        if(audioSource == bgMusicSource) bgPaused = false;

        while (audioSource.volume < targetVol)
        {
            audioSource.volume += targetVol * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.volume = targetVol;
        fadingSources.Remove(audioSource);
    }
}
