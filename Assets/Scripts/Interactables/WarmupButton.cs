using UnityEngine;
using UnityEngine.LowLevel;

public class WarmupButton : Interactable
{
    [SerializeField] private Transform symbol;
    [SerializeField] private Material glowMaterial;

    [SerializeField] private AudioClip thudSfx;
    [SerializeField] private AudioClip warmupUpSfx;

    private void OnEnable()
    {
        TaskManager.PhaseChanged += OnPhaseChange;
    }

    private void OnPhaseChange()
    {
        if (TaskManager.Instance.currentPhase is WeaponTaskPhase)
        {
            foreach(Transform t in symbol)
            {
                t.GetComponent<Renderer>().material = glowMaterial;
            }
            isInteractable = true;
            TaskManager.PhaseChanged -= OnPhaseChange;
        }
    }

    protected override void Interact(Transform player)
    {
        base.Interact(player);
        player.GetComponent<PlayerLook>().CameraShake(5f, 2.5f, true);
        isInteractable = false;
        requirementsNotMetMessage = string.Empty;
        SoundManager.Instance.PlaySFXOneShot(thudSfx);
        SoundManager.Instance.PlaySFXOneShot(warmupUpSfx);
    }
}
