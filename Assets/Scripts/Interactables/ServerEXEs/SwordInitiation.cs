using UnityEngine;

public class SwordInitiation : ServerEXE
{
    private bool initiationDone = false;
    public override void RunExe()
    {
        TaskManager.Instance.CompleteTask(task);
        DoomsdayStatusUI.Instance.warmedUp = true;
        initiationDone = true;
        base.RunExe();
    }

    public override bool IsRunnable()
    {
        return !initiationDone;
    }
}
