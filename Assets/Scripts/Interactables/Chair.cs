using UnityEngine;

public class Chair : Interactable
{
    [SerializeField] private Transform sitPoint;
    [SerializeField] private AudioClip sfx;

    protected override void Interact(Transform player)
    {
        player.GetComponent<PlayerMotor>().ForcePlayerToPoint(sitPoint, true);
        SoundManager.Instance.PlaySFXOneShot(sfx);
    }
}
