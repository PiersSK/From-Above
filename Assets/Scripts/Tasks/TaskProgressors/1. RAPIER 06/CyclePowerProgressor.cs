public class CyclePowerProgressor : TaskProgressor
{
    protected override void OnEnable()
    {
        base.OnEnable();
        Interactable.PlayerInteracted += CycleButtonPressed;
    }

    private void CycleButtonPressed(Interactable interactable)
    {
        if (interactable is CyclePowerButton)
        {
            TaskManager.Instance.ProgressTask(task);
            Interactable.PlayerInteracted -= CycleButtonPressed;
        }
    }
}
