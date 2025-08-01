using UnityEngine;

public class ShareMaintenanceProgressor : TaskProgressor
{
    [SerializeField] private DataDrive rapierDrive;
    [SerializeField] private ServerExe receiveExe;
    [SerializeField] private ServerExe transmitExe;
    [SerializeField] private DiscSlotContent rapierTwoContent;

    private bool receieveLoaded = false;
    private bool transmitLoaded = false;
    private bool rapierDriveInserted = false;
    private bool rapierDataReceived = false;
    private bool rapierDataUploaded = false;

    // Steps:
    // 0) Load 'Receive FC' and 'RAPIER Project PD' into server hub
    // 1) Receive data from RAPIER02
    // 2) Load 'Transmit FC' into server hub
    // 3) Transmit data received from RAPIER02 to command

    private void Update()
    {
        if ((TaskManager.Instance.currentPhase.GetActiveTask(task) != null))
        {
            switch (TaskManager.Instance.currentPhase.GetActiveTask(task).currentStep)
            {
                case 0:
                    if (receieveLoaded && rapierDriveInserted) TaskManager.Instance.ProgressTask(task);
                    break;
                case 1:
                    if (rapierDataReceived) TaskManager.Instance.ProgressTask(task);
                    break;
                case 2:
                    if (transmitLoaded) TaskManager.Instance.ProgressTask(task);
                    break;
                case 3:
                    if (rapierDataUploaded) TaskManager.Instance.ProgressTask(task);
                    break;
                default:
                    break;
            }
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (!(receieveLoaded && transmitLoaded)) EXEStorage.EXEStorageChanged += RequiredExesLoaded;
        if (!rapierDriveInserted) PDStorage.PDStorageChanged += RapierDriveLoaded;
        if (!rapierDataReceived) FuncCardUI.FunctionExecuted += FunctionsExecuted;
    }

    private void FunctionsExecuted(FuncCardUI func, IServerDataObject pd, DiscSlotContent content)
    {
        if (func is FuncReceiveUI && pd == rapierDrive) rapierDataReceived = true;
        else if (func is FuncTransmitUI && content == rapierTwoContent) rapierDataUploaded = true;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EXEStorage.EXEStorageChanged -= RequiredExesLoaded;
        PDStorage.PDStorageChanged -= RapierDriveLoaded;
    }

    private void RapierDriveLoaded(IServerDataObject dataObj)
    {
        if(dataObj == rapierDrive)
        {
            rapierDriveInserted = true;
            PDStorage.PDStorageChanged -= RapierDriveLoaded;
        }
    }

    private void RequiredExesLoaded(IServerDataObject dataObj)
    {
        if (dataObj == receiveExe) receieveLoaded = true;
        else if (dataObj == transmitExe) transmitLoaded = true;

        if(receieveLoaded && transmitLoaded) EXEStorage.EXEStorageChanged -= RequiredExesLoaded;
    }
}
