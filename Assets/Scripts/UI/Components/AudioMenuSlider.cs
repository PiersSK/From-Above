using UnityEngine;
using UnityEngine.Audio;

public class AudioMenuSlider : MenuSlider
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private MixerGroup targetValue;
    private enum MixerGroup
    {
        masterVolume,
        sfxVolume,
        bgMusicVolume
    }
    private void Start()
    {
        slider.onValueChanged.AddListener(UpdateAudioVolume);
        UpdateAudioVolume(slider.value);
    }

    private void UpdateAudioVolume(float value)
    {
        if (mixer == null) return;

        float modifiedValue = value == 0 ? -80f : actualValueOffset + actualValuePerIncrement * value;
        mixer.SetFloat(targetValue.ToString(), modifiedValue);
    }
}
