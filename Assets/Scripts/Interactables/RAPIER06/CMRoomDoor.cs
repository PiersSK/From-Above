using UnityEngine;

public class CMRoomDoor : Door
{
    [SerializeField] private GameObject doorSign;
    [SerializeField] private GameObject floorSign;
    private bool hasBeenOpened = false;

    public override void UnlockAndOpenDoor()
    {
        if(!hasBeenOpened)
        {
            doorSign.SetActive(false);
            floorSign.GetComponent<Renderer>().enabled = true;
            hasBeenOpened = true;
        }

        base.UnlockAndOpenDoor();
    }
}
