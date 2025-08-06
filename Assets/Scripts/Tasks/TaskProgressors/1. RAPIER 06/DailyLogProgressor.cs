using UnityEngine;

public class DailyLogProgressor : TaskProgressor
{
    [SerializeField] private DataDrive logDrive;
    [SerializeField] private ServerExe transmitExe;
    [SerializeField] private DiscSlotContent logContent;
    [SerializeField] private DiaryTerminal diaryTerminal;

    private bool transmitLoaded = false;
    private bool logDriveInserted = false;
    private bool logDataUploaded = false;

    private void Update()
    {
        if ((TaskManager.Instance.currentPhase.GetActiveTask(task) != null))
        {
            switch (TaskManager.Instance.currentPhase.GetActiveTask(task).currentStep)
            {
                case 0:
                    if (diaryTerminal.logCompleted) TaskManager.Instance.ProgressTask(task);
                    break;
                case 1:
                    if (transmitLoaded && logDriveInserted) TaskManager.Instance.ProgressTask(task);
                    break;
                case 2:
                    if (logDataUploaded) TaskManager.Instance.ProgressTask(task);
                    break;
                default:
                    break;
            }
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (!transmitLoaded) EXEStorage.EXEStorageChanged += RequiredExesLoaded;
        if (!logDriveInserted) PDStorage.PDStorageChanged += DriveInserted;
        if (!logDataUploaded) FuncCardUI.FunctionExecuted += FunctionsExecuted;
    }

    private void FunctionsExecuted(FuncCardUI func, IServerDataObject pd, DiscSlotContent content)
    {
        if (func is FuncTransmitUI && content == logContent)
        {
            logDataUploaded = true;
            FuncCardUI.FunctionExecuted -= FunctionsExecuted;
        }

    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EXEStorage.EXEStorageChanged -= RequiredExesLoaded;
        PDStorage.PDStorageChanged -= DriveInserted;
        FuncCardUI.FunctionExecuted -= FunctionsExecuted;
    }

    private void DriveInserted(IServerDataObject dataObj)
    {
        if (dataObj == logDrive)
        {
            logDriveInserted = true;
            PDStorage.PDStorageChanged -= DriveInserted;
        }
    }

    private void RequiredExesLoaded(IServerDataObject dataObj)
    {
        if (dataObj == transmitExe)
        {
            transmitLoaded = true;
            EXEStorage.EXEStorageChanged -= RequiredExesLoaded;
        }
    }
}
