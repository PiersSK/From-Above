using UnityEngine;

public class DeployWheel : HoldInteractable
{
    [SerializeField] private Animator anim;
    [SerializeField] private Task task;
    [SerializeField] private PlayerLook look;
    [SerializeField] private GameObject wheel;
    private bool fullTurnComplete = false;

    [SerializeField] private Animator weaponAnim;
    [SerializeField] private AudioClip sfx;
    [SerializeField] private AudioClip deploySound;
    [SerializeField] private AudioClip confirmVoice;

    public override bool CanInteract()
    {
        return !fullTurnComplete && TaskManager.Instance.currentPhase.tasks.Contains(task);
    }

    private void Update()
    {
        if (wheel.transform.rotation.z == 360 && !fullTurnComplete)
        {
            fullTurnComplete = true;
            DoomsdayStatusUI.Instance.weaponLeversPulled++;
            SoundManager.Instance.PlaySFXOneShot(sfx, 0, 0.3f);

            if (DoomsdayStatusUI.Instance.weaponLeversPulled == 2)
            {
                TaskManager.Instance.CompleteTask(task);
                SoundManager.Instance.PlaySFXOneShot(deploySound, 0, 0.5f);
                SoundManager.Instance.PlayShipPALine(confirmVoice);
                InputManager.Instance.ClearInteractHooks(this);
            }
        }
    }

    protected override void Interact(Transform player)
    {
        weaponAnim.SetTrigger("Deploy");
        anim.SetTrigger("TurnWheel");

        weaponAnim.speed = 1;
        anim.speed = 1;
    }

    protected override void CancelInteract(Transform player)
    {
        Debug.Log("WEAPON DEPLOY WHEEL INTERACT CANCELLING");
        weaponAnim.speed = 0;
        anim.speed = 0;

        base.CancelInteract(player);
    }
}
