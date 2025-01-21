using UnityEngine;

public class Lever : Interactable
{
    [SerializeField] private Animator anim;
    private bool leverIsUp = false;

    protected override void Interact(Transform player)
    {
        leverIsUp = !leverIsUp;
        anim.SetBool("LeverUp", leverIsUp);
    }
}
