using UnityEngine;

public class ProcessFleetDataProgressor : TaskProgressor
{
    [SerializeField] private DataDrive rapierFleetDrive;

    private void OnEnable()
    {
        ServerDiscStorage.PDEjected += FleetDataEjected;
        BridgeTerminal.DataUploaded += FleetDataUploaded;
        TaskManager.TaskCompleted += IsTaskCompleted;
    }

    private void IsTaskCompleted(TaskData task)
    {
        if(this.task == task)
        {
            TaskManager.TaskCompleted -= IsTaskCompleted;
            Destroy(gameObject);
        }
    }

    private void FleetDataEjected(DataDrive drive)
    {
        if (drive == rapierFleetDrive) TaskManager.Instance.ProgressTask(task);
        ServerDiscStorage.PDEjected -= FleetDataEjected;
    }

    private void FleetDataUploaded(DataDrive drive)
    {
        if (drive == rapierFleetDrive) TaskManager.Instance.ProgressTask(task);
        BridgeTerminal.DataUploaded -= FleetDataUploaded;
    }

    private void OnDisable()
    {
        ServerDiscStorage.PDEjected -= FleetDataEjected;
        BridgeTerminal.DataUploaded -= FleetDataUploaded;
        TaskManager.TaskCompleted -= IsTaskCompleted;
    }
}
