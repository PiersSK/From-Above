using UnityEngine;

public class ProcessFleetDataProgressor : TaskProgressor
{
    [SerializeField] private DataDrive rapierFleetDrive;
    [SerializeField] private IServerDataObject decipherAlphaFC;
    [SerializeField] private IServerDataObject transmitFC;
    [SerializeField] private DiscSlotContent rapierFleetData;

    private bool discEjectedFromServer = false;
    private bool discInServerHub = false;
    private bool decipherInServerHub = false;
    private bool dataDecrypted = false;
    private bool transmitInServerHub = false;
    private bool dataTransmitted = false;

    private void Update()
    {
        if ((TaskManager.Instance.currentPhase.GetActiveTask(task) != null))
        {
            switch (TaskManager.Instance.currentPhase.GetActiveTask(task).currentStep)
            {
                case 0:
                    if (discEjectedFromServer) TaskManager.Instance.ProgressTask(task);
                    break;
                case 1:
                    if (discInServerHub && decipherInServerHub) TaskManager.Instance.ProgressTask(task);
                    break;
                case 2:
                    if (dataDecrypted) TaskManager.Instance.ProgressTask(task);
                    break;
                case 3:
                    if (transmitInServerHub) TaskManager.Instance.ProgressTask(task);
                    break;
                case 4:
                    if (dataTransmitted) TaskManager.Instance.ProgressTask(task);
                    break;
                default:
                    break;
            }
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if(!discEjectedFromServer) ServerDiscStorage.PDEjected += FleetDataEjected;
        if (!(decipherInServerHub && transmitInServerHub)) EXEStorage.EXEStorageChanged += RequiredExesLoaded;
        if (!discInServerHub) PDStorage.PDStorageChanged += RapierDriveLoaded;
        if (!(dataDecrypted && dataTransmitted)) FuncCardUI.FunctionExecuted += FunctionsExecuted;
    }

    private void FleetDataEjected(DataDrive drive)
    {
        if (drive == rapierFleetDrive)
        {
            ServerDiscStorage.PDEjected -= FleetDataEjected;
            discEjectedFromServer = true;
        }
    }

    private void RequiredExesLoaded(IServerDataObject dataObj)
    {
        if (dataObj == decipherAlphaFC) decipherInServerHub = true;
        else if (dataObj == transmitFC) transmitInServerHub = true;

        if (decipherInServerHub && transmitInServerHub) EXEStorage.EXEStorageChanged -= RequiredExesLoaded;
    }

    private void RapierDriveLoaded(IServerDataObject dataObj)
    {
        if (dataObj == rapierFleetDrive)
        {
            discInServerHub = true;
            PDStorage.PDStorageChanged -= RapierDriveLoaded;
        }
    }

    private void FunctionsExecuted(FuncCardUI func, IServerDataObject pd, DiscSlotContent content)
    {
        if (func is FuncDecipherUI && content == rapierFleetData) dataDecrypted = true;
        else if (func is FuncTransmitUI && content == rapierFleetData) dataTransmitted = true;

        if(dataDecrypted && dataTransmitted) FuncCardUI.FunctionExecuted -= FunctionsExecuted;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        ServerDiscStorage.PDEjected -= FleetDataEjected;
        EXEStorage.EXEStorageChanged -= RequiredExesLoaded;
        PDStorage.PDStorageChanged -= RapierDriveLoaded;
        FuncCardUI.FunctionExecuted -= FunctionsExecuted;
    }
}
