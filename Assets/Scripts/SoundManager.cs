using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    public AudioSource genericSFXSource;
    public string clipPlaying = string.Empty;

    private void Awake()
    {
        Instance = this;
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
}
