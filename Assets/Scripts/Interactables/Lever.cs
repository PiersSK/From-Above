using UnityEngine;

public class Lever : Interactable
{
    [SerializeField] private Animator anim;
    [SerializeField] private Task task;
    [SerializeField] private PlayerLook look;
    private bool leverIsUp = false;

    [SerializeField] private Animator weaponAnim;
    [SerializeField] private AudioClip sfx;
    [SerializeField] private AudioClip deploySound;
    [SerializeField] private AudioClip confirmVoice;

    public override bool CanInteract()
    {
        return !leverIsUp && TaskManager.Instance.tasks.Contains(task);
    }

    protected override void Interact(Transform player)
    {
        leverIsUp = true;
        anim.SetBool("LeverUp", leverIsUp);
        DoomsdayStatusUI.Instance.weaponLeversPulled++;
        SoundManager.Instance.PlaySFXOneShot(sfx, 0, 0.3f);

        if(DoomsdayStatusUI.Instance.weaponLeversPulled == 3)
        {
            TaskManager.Instance.CompleteTask(task);
            weaponAnim.SetTrigger("Deploy");
            SoundManager.Instance.PlaySFXOneShot(deploySound, 0, 0.5f);
            SoundManager.Instance.PlayShipPALine(confirmVoice);
            look.CameraShake(19f, 0.5f);
        }
    }
}
