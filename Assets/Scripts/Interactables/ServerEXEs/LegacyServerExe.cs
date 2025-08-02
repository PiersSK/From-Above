using UnityEngine;

public class LegacyServerExe : MonoBehaviour
{
    // USED IN CURRENT SWORD WARMUP TASK ONLY BUT SCHEDULED FOR REMOVAL UPON UPDATE OF THAT TASK
    // TODO: Remove this once sword update task is updated

    public TaskData task;
    public bool hasRun = false;
    public virtual void RunExe()
    {
        hasRun = true;
    }

    public virtual bool IsRunnable()
    {
        return true;
    }
}
