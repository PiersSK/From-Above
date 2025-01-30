using UnityEngine;

public class ClearableInteractable : Interactable
{
    [SerializeField] private RoomTidyCounter tidy;
    [SerializeField] private AudioClip sfx;

    protected override void Interact(Transform player)
    {
        if(tidy != null) tidy.objectsRemoved++;
        gameObject.SetActive(false);
        if(sfx != null) SoundManager.Instance.PlaySFXOneShot(sfx, 0f, 0.4f);
    }
}
