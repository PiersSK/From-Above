using System.Collections.Generic;
using UnityEngine;

public class FireButton : Interactable
{
    [SerializeField] private Task task;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private AudioClip fireCountdown;
    [SerializeField] private AudioClip sfx;
    [SerializeField] private Animator anim;

    [SerializeField] List<AudioSource> commanderLines;

    public override bool CanInteract()
    {
        return TaskManager.Instance.tasks.Contains(task);
    }

    protected override void Interact(Transform player)
    {
        foreach (AudioSource source in commanderLines) source.enabled = false;

        anim.SetTrigger("Press");
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
