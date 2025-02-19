using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : Interactable
{
    [SerializeField] private List<ShipLight> lights;
    [SerializeField] private Animator anim;
    [SerializeField] private bool isOn = false;

    [SerializeField] private AudioClip switchSound;

    private void Start()
    {
        anim.SetBool("IsOn", isOn);
    }

    protected override void Interact(Transform player)
    {
        foreach (var light in lights)
        {
            light.ToggleLight();
        }

        isOn = !isOn;
        anim.SetBool("IsOn", isOn);
        SoundManager.Instance.PlaySFXOneShot(switchSound);
    }
}
