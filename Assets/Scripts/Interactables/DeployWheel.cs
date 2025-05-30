using UnityEngine;

public class DeployWheel : Interactable
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

    protected override void Interact(Transform player)
    {
        weaponAnim.SetTrigger("Deploy");

        if (wheel.transform.rotation.z == 360)
        {
            fullTurnComplete = true;
            anim.SetBool("FullTurnComplete", fullTurnComplete);
            DoomsdayStatusUI.Instance.weaponLeversPulled++;
            SoundManager.Instance.PlaySFXOneShot(sfx, 0, 0.3f);

            if (DoomsdayStatusUI.Instance.weaponLeversPulled == 2)
            {
                TaskManager.Instance.CompleteTask(task); 
                SoundManager.Instance.PlaySFXOneShot(deploySound, 0, 0.5f);
                SoundManager.Instance.PlayShipPALine(confirmVoice);
            }
        }

        
    }
}
