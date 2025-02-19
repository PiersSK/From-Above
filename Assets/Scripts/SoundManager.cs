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
    private List<AudioSource> fadingSources = new();

    public string clipPlaying = string.Empty;

    private float soundtrackTimer = 0f;
    private int ambientSpacing;
    public int currentBGClip = 0;
    private bool bgPaused = false;
    [SerializeField] private List<AudioClip> soundtrack;
    [SerializeField] private AudioClip noWeaponEnding;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        bgVol = bgMusicSource.volume;
        ambientSpacing = Random.Range(30, 100);
        Debug.Log("ambient will play after " + ambientSpacing);
    }

    private void Update()
    {
        if (!genericSFXSource.isPlaying) clipPlaying = string.Empty;

        if(!fadingSources.Contains(bgMusicSource)) AutoAdjustBGForOtherTracks();

        soundtrackTimer += Time.deltaTime;
        UpdateScore();
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

    private void UpdateScore()
    {

        if (currentBGClip == 0)
        {
            if (!bgMusicSource.isPlaying && !bgPaused)
            {
                currentBGClip = 1;
                soundtrackTimer = 0;
            }

            if(TaskManager.Instance.isPhaseTwo)
            {
                soundtrackTimer = 0;
                currentBGClip = 2;
                bgMusicSource.Stop();
                bgMusicSource.PlayOneShot(soundtrack[2]);
                Invoke("IncrementSoundtrack", soundtrack[2].length);
            }
        }
        else if (currentBGClip == 1)
        {
            if (soundtrackTimer >= ambientSpacing)
            {
                soundtrackTimer = 0;
                ambientSpacing = Random.Range(30, 100);
                Debug.Log("ambient will play again after " + ambientSpacing);
                bgMusicSource.PlayOneShot(soundtrack[1]);
            }

            if (TaskManager.Instance.isPhaseTwo)
            {
                soundtrackTimer = 0;
                currentBGClip = 2;
                bgMusicSource.PlayOneShot(soundtrack[2]);
                Invoke("IncrementSoundtrack", soundtrack[2].length);
            }
        } else if (currentBGClip == 3)
        {
            if(!bgMusicSource.isPlaying)
            {
                bgMusicSource.clip = soundtrack[3];
                bgMusicSource.loop = true;
                bgMusicSource.Play();
            }

            if(TaskManager.Instance.phaseTwoTasksCompleted >= 3 || TimeController.Instance.GetTimeInSeconds() > TimeController.Instance.phase2TimeLimitMins * 0.8 * 60)
            {
                currentBGClip = 4;
                bgMusicSource.clip = soundtrack[4];
                bgMusicSource.Play();
            }
        } else if (currentBGClip == 4)
        {
            if(TaskManager.Instance.phaseTwoTasksCompleted == 6 || TimeController.Instance.GetTimeInSeconds() >= TimeController.Instance.phase2TimeLimitMins * 60 - 12)
            {
                Debug.Log("Moving to final soundtrack");
                currentBGClip = 5;
                bgMusicSource.clip = soundtrack[5];
                bgMusicSource.loop = false;
                bgMusicSource.Play();
                Invoke("IncrementSoundtrack", soundtrack[5].length);

            }
        } else if (currentBGClip ==  6)
        {
            if(TaskManager.Instance.pacifistEndingReached)
            {
                bgMusicSource.clip = noWeaponEnding;
                bgMusicSource.loop = false;
                bgMusicSource.Play();
                currentBGClip = -1;
                Invoke("ShowPacifistEnding", noWeaponEnding.length - 12f);
            }
        }
    }

    private void ShowPacifistEnding()
    {
        UIManager.Instance.ShowPacifistEnding();
    }

    private void IncrementSoundtrack()
    {
        currentBGClip++;
        soundtrackTimer = 0f;
    }
}
