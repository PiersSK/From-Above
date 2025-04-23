using UnityEngine;

public abstract class ITaskPad : MonoBehaviour
{
    protected abstract void updateTaskPadUI();

    protected abstract void completeTask();
}
