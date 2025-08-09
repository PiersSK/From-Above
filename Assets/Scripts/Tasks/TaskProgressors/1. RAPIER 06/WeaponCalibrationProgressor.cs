using UnityEngine;

public class WeaponCalibrationProgressor : TaskProgressor
{
    [SerializeField] private CalibrationUI calUI;
    [SerializeField] private CalibrationTerminal calTerminal;
    [SerializeField] private TaskData warmupTask;

    private void Update()
    {
        if(calUI.calibrationCompleted)
        {
            TaskManager.Instance.ProgressTask(task);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        TaskManager.PhaseChanged += TaskManager_PhaseChanged;
        TaskManager.TaskCompleted += TaskManager_TaskCompleted;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        TaskManager.PhaseChanged -= TaskManager_PhaseChanged;
        TaskManager.TaskCompleted -= TaskManager_TaskCompleted;
    }

    private void TaskManager_TaskCompleted(TaskData task)
    {
        if(task == warmupTask)
        {
            calUI.EnableWeaponCalibration();
            calTerminal.InitialEnable();
            TaskManager.TaskCompleted -= TaskManager_TaskCompleted;
        }
    }

    private void TaskManager_PhaseChanged()
    {
        calUI.ConfirmTargetStatus();
        TaskManager.PhaseChanged -= TaskManager_PhaseChanged;
    }
}
