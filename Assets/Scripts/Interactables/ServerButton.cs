using UnityEngine;

public class ServerButton : Interactable
{
    public ServerEXE exe;
    public bool usableInPhaseOne = false;

    public override bool CanInteract()
    {

        return base.CanInteract() && (usableInPhaseOne || TaskManager.Instance.isPhaseTwo) && exe != null && exe.IsRunnable();
    }

    protected override void Interact(Transform player)
    {
        exe.RunExe();
    }
}
