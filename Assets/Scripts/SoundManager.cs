using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    public AudioSource genericSFXSource;

    private void Awake()
    {
        Instance = this;
    }

    public void PlaySFXOneShot(AudioClip clip, float maxPitchVariation = 0f, float volume = 1f, float maxVolumeVariation = 0f)
    {
        maxPitchVariation = Mathf.Clamp(maxPitchVariation, 0f, 3f);
        maxVolumeVariation = Mathf.Clamp(maxVolumeVariation, 0f, 0.5f);
        genericSFXSource.volume = volume + Random.Range(-maxVolumeVariation, maxVolumeVariation);
        genericSFXSource.pitch = 1 + Random.Range(-maxPitchVariation, maxPitchVariation);
        genericSFXSource.PlayOneShot(clip);
    }
}
