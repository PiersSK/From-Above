using UnityEngine;

public class ConsoleLever : Interactable
{
    public bool isFlipped = false;

    [SerializeField] private ConsoleLever otherLever;
    [SerializeField] private Task task;
    [SerializeField] private AudioClip sfx;
    [SerializeField] private AudioClip taskCompleteLine;

    protected override void Interact(Transform player)
    {
        GetComponent<Animation>().Play();
        SoundManager.Instance.PlaySFXOneShot(sfx, 0, 0.3f);
        isFlipped = true;
        DoomsdayStatusUI.Instance.calibrated++;

        if (otherLever.isFlipped)
        {
            TaskManager.Instance.CompleteTask(task);
            SoundManager.Instance.PlaySFXOneShot(taskCompleteLine, 0, 0.3f);
        }
    }

    public override bool CanInteract()
    {
        return TaskManager.Instance.isPhaseTwo && !isFlipped && TaskManager.Instance.tasks.Contains(task);
    }
}
