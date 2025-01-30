using UnityEngine;

public class SwordInitiation : ServerEXE
{
    private bool initiationDone = false;
    [SerializeField] private AudioClip warmupSfx;

    public override void RunExe()
    {
        TaskManager.Instance.CompleteTask(task);
        DoomsdayStatusUI.Instance.warmedUp = true;
        initiationDone = true;
        SoundManager.Instance.PlaySFXOneShot(warmupSfx, 0, 0.3f);
        base.RunExe();
    }

    public override bool IsRunnable()
    {
        return !initiationDone;
    }
}
