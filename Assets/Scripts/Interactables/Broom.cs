using UnityEngine;

public class Broom : ClearableInteractable
{
    protected override void Interact(Transform player)
    {
        GetComponent<Animator>().SetTrigger("Sweep");
        base.Interact(player);
    }
}
