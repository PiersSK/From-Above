using System.Collections.Generic;
using UnityEngine;
using static LocalContent;

public class ShipDoorController : MonoBehaviour
{
    public static ShipDoorController Instance;

    public Door bridgeDoor;
    public Door lq1Door;
    public Door lq2Door;
    public Door healthDoor;
    public Door utilityDoor;
    public Door engineDoor;
    public Door doomsdayDoor;

    private List<Door> overriddenLockedDoors = new();

    private void Awake()
    {
        Instance = this;
    }

    private void RemoteLockIfUnlocked(Door door)
    {
        if(!door.isLocked)
        {
            door.LockDoor();
            overriddenLockedDoors.Add(door);
        }
    }

    public void OverrideLockAllDoors()
    {
        RemoteLockIfUnlocked(bridgeDoor);
        RemoteLockIfUnlocked(lq1Door);
        RemoteLockIfUnlocked(lq2Door);
        RemoteLockIfUnlocked(healthDoor);
        RemoteLockIfUnlocked(utilityDoor);
        RemoteLockIfUnlocked(engineDoor);
        RemoteLockIfUnlocked(doomsdayDoor);
    }

    public void UnlockAllOverriddenDoors()
    {
        foreach(Door d in overriddenLockedDoors)
        {
            d.UnlockDoor();
        }

        overriddenLockedDoors = new();
    }


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

    public void RemoteClose(LocalLocations location)
    {
        switch (location)
        {
            case LocalLocations.Bridge:
                bridgeDoor.CloseDoor();
                return;
            case LocalLocations.LivingQuartersOne:
                lq1Door.CloseDoor();
                return;
            case LocalLocations.LivingQuartersTwo:
                lq2Door.CloseDoor();
                return;
            case LocalLocations.Health:
                healthDoor.CloseDoor();
                return;
            case LocalLocations.Utility:
                utilityDoor.CloseDoor();
                return;
            case LocalLocations.EngineRoom:
                engineDoor.CloseDoor();
                return;
            case LocalLocations.DoomsdayRoom:
                doomsdayDoor.CloseDoor();
                return;
            default:
                return;
        }
    }
}
