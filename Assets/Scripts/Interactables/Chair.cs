using UnityEngine;

public class Chair : Interactable
{
    [SerializeField] private Transform sitPoint;

    protected override void Interact(Transform player)
    {
        player.GetComponent<PlayerMotor>().ForcePlayerToPoint(sitPoint, true);
    }
}
