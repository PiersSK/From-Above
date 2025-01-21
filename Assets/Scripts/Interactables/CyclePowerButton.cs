using UnityEngine;

public class CyclePowerButton : Interactable
{
    [SerializeField] private Animation lightFlicker;
    [SerializeField] private PlayerLook playerLook;
    private Animator buttonAnimator;

    private void Start()
    {
        buttonAnimator = GetComponent<Animator>();
    }

    protected override void Interact(Transform player)
    {
        buttonAnimator.SetTrigger("Press");
        lightFlicker.Play();
        playerLook.CameraShake(5f, 5f, true);
    }
}
