using UnityEngine;

public class PickupKeycard : Interactable
{
    public enum KeyCard
    {
        One,
        Two
    }
    public KeyCard key;
    [SerializeField] private AudioClip sfx;

    protected override void Interact(Transform player)
    {
        if(key == KeyCard.One)
        {
            PlayerInventory.Instance.hasKeycard1 = true;
        } else if(key == KeyCard.Two)
        {
            PlayerInventory.Instance.hasKeycard2 = true;
        }

        isInteractable = false;
        gameObject.SetActive(false);
        SoundManager.Instance.PlaySFXOneShot(sfx);
    }
}
