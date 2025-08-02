using UnityEngine;

public class DeployWheel : HoldInteractable
{
    private Animator anim;
    [SerializeField] private TaskData task;
    [SerializeField] private PlayerLook look;

    private bool animTriggered = false;
    private bool fullTurnComplete = false;

    [SerializeField] private Animator weaponAnim;
    [SerializeField] private AudioClip sfx;
    [SerializeField] private AudioClip deploySound;
    [SerializeField] private AudioClip confirmVoice;

    public override bool CanInteract()
    {
        return !fullTurnComplete && TaskManager.Instance.currentPhase.tasks.Contains(task);
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void WheelTurnComplete() 
    {
        PlayerMotor.Instance.SetPlayerLock(false);
        PlayerLook.Instance.SetLookLock(false, false);
        DoomsdayStatusUI.Instance.weaponLeversPulled++;
        fullTurnComplete = true;
        SoundManager.Instance.PlaySFXOneShot(sfx, 0, 0.3f);

        if (DoomsdayStatusUI.Instance.weaponLeversPulled == 2)
        {
            TaskManager.Instance.ProgressTask(task);
            SoundManager.Instance.PlaySFXOneShot(deploySound, 0, 0.5f);
            SoundManager.Instance.PlayShipPALine(confirmVoice);
            InputManager.Instance.ClearInteractHooks(this);
        }
    }

    protected override void Interact(Transform player)
    {
        if(!animTriggered)
        {
            animTriggered = true; 
            weaponAnim?.SetTrigger("Deploy");
            anim?.SetBool("TurnWheel", animTriggered);
        }
        
        weaponAnim.speed = 1;
        anim.speed = 1;

        base.Interact(player);
    }

    protected override void CancelInteract(Transform player)
    {
        weaponAnim.speed = 0;
        anim.speed = 0;

        base.CancelInteract(player);
    }
}
