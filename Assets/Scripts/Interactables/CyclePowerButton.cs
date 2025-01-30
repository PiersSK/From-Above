using UnityEngine;

public class CyclePowerButton : Interactable
{
    [SerializeField] private Animation lightFlicker;
    [SerializeField] private PlayerLook playerLook;
    [SerializeField] private GameObject cycleNotif;
    private Animator buttonAnimator;

    [SerializeField] int dangerRange = 43;
    [SerializeField] GameObject overloadMsg;
    private int timesCycledInRange = 0;
    private float timer = 0f;

    private bool inProgress;
    public bool overloaded = false;

    [SerializeField] private AudioClip buttonSfx;
    [SerializeField] private AudioClip cycleSequenceSfx;

    [SerializeField] private Task task;

    private void Start()
    {
        buttonAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(timesCycledInRange > 0)
        {
            timer += Time.deltaTime;
            if(timer > dangerRange)
            {
                timer = 0f;
                timesCycledInRange = 0;
            }
        }

        if(timesCycledInRange == 3 && !overloaded)
        {
            overloaded = true;
            overloadMsg.SetActive(true);
        }
    }

    public override bool CanInteract()
    {
        return !inProgress;
    }

    protected override void Interact(Transform player)
    {
        timesCycledInRange++;

        buttonAnimator.SetTrigger("Press");
        SoundManager.Instance.PlaySFXOneShot(buttonSfx);

        lightFlicker.Play();
        playerLook.CameraShake(5f, 2.5f * timesCycledInRange, true);
        TaskManager.Instance.CompleteTask(task);
        cycleNotif.SetActive(false);
        SoundManager.Instance.PlaySFXOneShot(cycleSequenceSfx, 0.2f * (timesCycledInRange-1), 0.3f + (0.1f * timesCycledInRange));

        inProgress = true;
        Invoke("EndCycle", 5f);
    }

    private void EndCycle()
    {
        inProgress = false;
    }
}
