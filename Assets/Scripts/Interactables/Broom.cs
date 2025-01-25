using UnityEngine;

public class Broom : Interactable
{
    [SerializeField] private RoomTidyCounter tidy;
    protected override void Interact(Transform player)
    {
        GetComponent<Animator>().SetTrigger("Sweep");
        tidy.swept = true;
    }
}
