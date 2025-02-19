public class ServerPassword : FocusPickup
{
        public override void ReleasePlayer()
    {
        base.ReleasePlayer();
        PlayerInventory.Instance.hasServerPassword = true;
        isInteractable = false;
        gameObject.SetActive(false);
        SoundManager.Instance.PlaySFXOneShot(sfx);
    }
}