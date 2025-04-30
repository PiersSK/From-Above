using UnityEngine;

public class SwordInitiation : LegacyServerExe
{
    private bool initiationDone = false;
    [SerializeField] private AudioClip warmupSfx;

    public override void RunExe()
    {
        TaskManager.Instance.CompleteTask(task);
        DoomsdayStatusUI.Instance.warmedUp = true;
        initiationDone = true;
        SoundManager.Instance.PlayShipPALine(warmupSfx);
        base.RunExe();
    }

    public override bool IsRunnable()
    {
        return !initiationDone;
    }
}
