using UnityEngine;

public class CyclePowerButton : Interactable
{
    [SerializeField] private Animation lightFlicker;
    [SerializeField] private PlayerLook playerLook;
    [SerializeField] private GameObject cycleNotif;
    private Animator buttonAnimator;
    private bool powerNeedsCycling = true;

    [SerializeField] private AudioClip buttonSfx;
    [SerializeField] private AudioClip cycleSequenceSfx;

    [SerializeField] private Task task;

    private void Start()
    {
        buttonAnimator = GetComponent<Animator>();
    }

    protected override void Interact(Transform player)
    {
        buttonAnimator.SetTrigger("Press");
        SoundManager.Instance.PlaySFXOneShot(buttonSfx);

        if (powerNeedsCycling)
        {
            lightFlicker.Play();
            playerLook.CameraShake(5f, 5f, true);
            powerNeedsCycling = false;
            TaskManager.Instance.CompleteTask(task);
            cycleNotif.SetActive(false);
            SoundManager.Instance.PlaySFXOneShot(cycleSequenceSfx);
        }
    }
}
