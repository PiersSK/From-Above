using UnityEngine;

public class LegacyServerExe : MonoBehaviour
{
    public Task task;
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
