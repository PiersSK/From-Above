using System.Threading.Tasks;
using UnityEngine;

public class ClearableInteractable : Interactable
{
    [SerializeField] private RoomTidyCounter tidy;
    [SerializeField] private AudioClip sfx;
    [SerializeField] private bool removable;

    protected override void Interact(Transform player)
    {
        if (tidy != null) {
            tidy.objectsCleaned++;
        }

        if(removable) {
          gameObject.SetActive(false);
        }
        if (sfx != null) {
            SoundManager.Instance.PlaySFXOneShot(sfx, 0f, 0.4f);
        } 
    }

  public override bool CanInteract()
  {
    return TaskManager.Instance.currentPhase is DownTimePhase;
  }
}
