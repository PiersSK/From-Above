using UnityEngine;

public class RadioSwitch : Interactable
{
    [SerializeField] private Radio radio;
    [SerializeField] private GameObject onDial;
    [SerializeField] private GameObject offDial;
    [SerializeField] private AudioClip sfx;


    public override string GetPrompt()
    {
        return radio.isOn ? "Turn Radio Off" : "Turn Radio On";
    }

    protected override void Interact(Transform player)
    {
        SoundManager.Instance.PlaySFXOneShot(sfx);
        radio.isOn = !radio.isOn;
        onDial.SetActive(radio.isOn);
        offDial.SetActive(!radio.isOn);
    }
}
