using UnityEngine;

public class ServerButton : Interactable
{
    public LegacyServerExe exe;
    public bool usableInPhaseOne = false;
    [SerializeField] private AudioClip sfx;

    public override bool CanInteract()
    {

        return base.CanInteract() && (usableInPhaseOne || TaskManager.Instance.currentPhase is WeaponTaskPhase) && exe != null && exe.IsRunnable();
    }

    protected override void Interact(Transform player)
    {
        SoundManager.Instance.PlaySFXOneShot(sfx, 0, 0.3f);
        exe.RunExe();
    }
}
