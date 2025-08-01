public class ActiveTask
{
    public int currentStep = 0;
    public bool isCompleted = false;
    public Task task;

    public ActiveTask(Task task)
    {
        this.task = task;
    }

    public bool ProgressTask()
    {
        currentStep++;
        return currentStep >= task.taskSteps.Count;
    }
}