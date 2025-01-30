using UnityEngine;

public class FireButton : Interactable
{
    [SerializeField] private Task task;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private AudioClip fireCountdown;
    [SerializeField] private AudioClip sfx;

    public override bool CanInteract()
    {
        return TaskManager.Instance.tasks.Contains(task);
    }

    protected override void Interact(Transform player)
    {
        TaskManager.Instance.CompleteTask(task);
        SoundManager.Instance.PlaySFXOneShot(sfx);
        SoundManager.Instance.PlaySFXOneShot(fireCountdown);
        Invoke("EndGame", fireCountdown.length);
    }

    private void EndGame()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
    }


}
