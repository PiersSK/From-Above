using UnityEngine;
using static LocalContent;

public class ShipDoorController : MonoBehaviour
{
    public Door bridgeDoor;
    public Door lq1Door;
    public Door lq2Door;
    public Door healthDoor;
    public Door utilityDoor;
    public Door engineDoor;
    public Door doomsdayDoor;

    public void RemoteUnlockAndOpen(LocalLocations location)
    {
        switch (location)
        {
            case LocalLocations.Bridge:
                bridgeDoor.UnlockAndOpenDoor();
                return;
            case LocalLocations.LivingQuartersOne:
                lq1Door.UnlockAndOpenDoor();
                return;
            case LocalLocations.LivingQuartersTwo:
                lq2Door.UnlockAndOpenDoor();
                return;
            case LocalLocations.Health:
                healthDoor.UnlockAndOpenDoor();
                return;
            case LocalLocations.Utility:
                utilityDoor.UnlockAndOpenDoor();
                return;
            case LocalLocations.EngineRoom:
                engineDoor.UnlockAndOpenDoor();
                return;
            case LocalLocations.DoomsdayRoom:
                doomsdayDoor.UnlockAndOpenDoor();
                return;
            default:
                return;
        }
                
    }
}
