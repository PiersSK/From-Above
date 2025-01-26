using UnityEngine;

public class ServerEXE : MonoBehaviour
{
    public Task task;
    public virtual void RunExe()
    {
        //Do nothing
    }

    public virtual bool IsRunnable()
    {
        return true;
    }
}
