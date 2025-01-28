using UnityEngine;

public class Lever : Interactable
{
    [SerializeField] private Animator anim;
    [SerializeField] private Task task;
    private bool leverIsUp = false;

    [SerializeField] private Animator weaponAnim;

    public override bool CanInteract()
    {
        return !leverIsUp && TaskManager.Instance.tasks.Contains(task);
    }

    protected override void Interact(Transform player)
    {
        leverIsUp = true;
        anim.SetBool("LeverUp", leverIsUp);
        DoomsdayStatusUI.Instance.weaponLeversPulled++;

        if(DoomsdayStatusUI.Instance.weaponLeversPulled == 3)
        {
            TaskManager.Instance.CompleteTask(task);
            weaponAnim.SetTrigger("Deploy");
        }
    }
}
