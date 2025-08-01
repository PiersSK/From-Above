using UnityEngine;

public class ProcessFleetDataProgressor : TaskProgressor
{
    [SerializeField] private DataDrive rapierFleetDrive;

    protected override void OnEnable()
    {
        base.OnEnable();
        ServerDiscStorage.PDEjected += FleetDataEjected;
        BridgeTerminal.DataUploaded += FleetDataUploaded;
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

    protected override void OnDisable()
    {
        base.OnDisable();
        ServerDiscStorage.PDEjected -= FleetDataEjected;
        BridgeTerminal.DataUploaded -= FleetDataUploaded;
    }
}
