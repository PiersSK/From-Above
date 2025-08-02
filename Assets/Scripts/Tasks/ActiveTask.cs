public class ActiveTask
{
    public int currentStep = 0;
    public bool isCompleted = false;
    public TaskData task;

    public ActiveTask(TaskData task)
    {
        this.task = task;
    }

    public bool ProgressTask()
    {
        currentStep++;
        return currentStep >= task.taskSteps.Count;
    }
}