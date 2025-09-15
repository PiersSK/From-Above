using UnityEngine;

public class ShipSystemsPower : PowerCategory
{
    protected override void CommitPowerLevelChange()
    {
        base.CommitPowerLevelChange();
        switch (powerLevel)
        {
            case 0:
                ShipDoorController.Instance.OverrideLockAllDoors();
                ShipComputerController.Instance.SwitchOffAllComputers();
                PowerRouterController.Instance.SetToLowPower();
                break;
            case 1:
                ShipComputerController.Instance.SwitchOnAllComputers();
                ShipDoorController.Instance.UnlockAllOverriddenDoors();
                ShipLightController.Instance.ShutDownAllShipLights();
                ShipLightController.Instance.SetSecondaryLightsToEmergency();
                PowerRouterController.Instance.SetToNormalPower();
                ShipLightController.Instance.SetEmergencyState(true);
                break;
            case 2:
                ShipLightController.Instance.RevertSecondaryLights();
                ShipLightController.Instance.RevertShipLightsToPreviousState();
                ShipLightController.Instance.SetShipLightsToDefaultIntensity();
                ShipLightController.Instance.SetEmergencyState(false);
                break;
            case 3:
                ShipLightController.Instance.SetShipLightsToBrightIntensity();
                break;
            default:
                Debug.LogError("ShipSystemsPower asked to update to impossible power level");
                break;
        }
    }
}
