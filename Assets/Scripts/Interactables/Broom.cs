using UnityEngine;

public class Broom : Interactable
{
    [SerializeField] private RoomTidyCounter tidy;
    [SerializeField] private AudioClip sfx;
    protected override void Interact(Transform player)
    {
        GetComponent<Animator>().SetTrigger("Sweep");
        SoundManager.Instance.PlaySFXOneShot(sfx,0,0.2f);
        tidy.swept = true;
    }
}
