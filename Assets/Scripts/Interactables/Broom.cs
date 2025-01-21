using UnityEngine;

public class Broom : Interactable
{
    protected override void Interact(Transform player)
    {
        GetComponent<Animator>().SetTrigger("Sweep");
    }
}
