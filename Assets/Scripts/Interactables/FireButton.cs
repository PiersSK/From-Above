using System.Collections.Generic;
using UnityEngine;

public class FireButton : Interactable
{
    [SerializeField] private Task task;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject gameOverActivistUI;
    [SerializeField] private AudioClip fireCountdown;
    [SerializeField] private AudioClip sfx;
    [SerializeField] private Animator anim;

    [SerializeField] private CyclePowerButton cycleBtn;
    [SerializeField] private PlayerLook look;

    public bool weaponFired = false;

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
        SoundManager.Instance.PlayShipPALine(fireCountdown);
        weaponFired = true;

        if (cycleBtn.overloaded)
        {
            look.CameraShake(fireCountdown.length, 10f, true);
            Invoke("EndGameActivist", fireCountdown.length);
        }
        else
        {
            Invoke("EndGame", fireCountdown.length);
        }
    }

    private void EndGame()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
    }

    private void EndGameActivist()
    {
        gameOverActivistUI.SetActive(true);
        Time.timeScale = 0f;
    }


}
