using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    public AudioSource genericSFXSource;

    private void Awake()
    {
        Instance = this;
    }

    public void PlaySFXOneShot(AudioClip clip)
    {
        genericSFXSource.pitch = 1;
        genericSFXSource.PlayOneShot(clip);
    }

    public void PlaySFXOneShotWithPitchVariation(AudioClip clip, float maxVariation)
    {
        maxVariation = Mathf.Clamp(maxVariation, 0f, 3f);
        genericSFXSource.pitch = 1 + Random.Range(-maxVariation, maxVariation);
        genericSFXSource.PlayOneShot(clip);
    }
}
