using UnityEngine;

public class ServerButton : Interactable
{
    public ServerEXE exe;
    public bool usableInPhaseOne = false;
    [SerializeField] private AudioClip sfx;

    public override bool CanInteract()
    {

        return base.CanInteract() && (usableInPhaseOne || TaskManager.Instance.isPhaseTwo) && exe != null && exe.IsRunnable();
    }

    protected override void Interact(Transform player)
    {
        SoundManager.Instance.PlaySFXOneShot(sfx, 0, 0.3f);
        exe.RunExe();
    }
}
