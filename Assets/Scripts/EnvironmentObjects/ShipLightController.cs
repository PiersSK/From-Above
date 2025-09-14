using System.Collections.Generic;
using UnityEngine;

public class ShipLightController : MonoBehaviour
{
    public static ShipLightController Instance;

    private ShipLight[] shipLights;
    [SerializeField] private List<Light> doorSpotLights = new();
    [SerializeField] private List<Light> ambientLights = new();
    [SerializeField] private List<Light> spotLights = new();
    [SerializeField] private List<Renderer> emergencyLights = new();
    [SerializeField] private List<Renderer> doorSigns = new();
    [SerializeField] private Material redGlowMat;
    [SerializeField] private Renderer fairyLights;

    private bool shipInEmergencyState = false;
    private Dictionary<Light, Color> previousColors = new();
    private Dictionary<Light, bool> previousSpotLightState = new();
    private Dictionary<Renderer, Material[]> previousMats = new();

    public void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        shipLights = FindObjectsByType<ShipLight>(FindObjectsSortMode.None);
    }

    public void SetShipLightsToDefaultIntensity()
    {
        foreach (ShipLight light in shipLights) light.SetLightToStartIntensity();
    }

    public void SetShipLightsToBrightIntensity()
    {
        foreach (ShipLight light in shipLights) light.SetLightToBright();
    }

    public void ShutDownAllShipLights()
    {
        foreach(ShipLight light in shipLights)
        {
            light.SetLightState(false);
            light.SetChangeableState(false);
        }
    }

    public void RevertShipLightsToPreviousState()
    {
        if (!shipInEmergencyState) return;
        foreach (ShipLight light in shipLights) light.RevertToPreviousState();
    }

    public void SetSecondaryLightsToEmergency()
    {
        shipInEmergencyState = true;
        Material[] tempMats;
        previousColors = new();
        previousMats = new();
        previousSpotLightState = new();

        foreach (Light l in doorSpotLights)
        {
            previousColors[l] = l.color;
            l.color = Color.red;
        }

        foreach (Light l in ambientLights)
        {
            previousColors[l] = l.color;
            l.color = Color.red;
        }

        foreach (Light l in spotLights)
        {
            previousColors[l] = l.color;
            previousSpotLightState[l] = l.enabled;
            l.enabled = true; 
            l.color = Color.red;
        }

        foreach (Renderer r in doorSigns)
        {
            previousMats[r] = r.materials;
            tempMats = r.materials;
            tempMats[1] = redGlowMat;
            r.materials = tempMats;
        }

        foreach (Renderer r in emergencyLights)
        {
            previousMats[r] = r.materials;
            tempMats = r.materials;
            tempMats[0] = redGlowMat;
            r.materials = tempMats;
        }

        // TODO: This feels very specific to RAPIER06
        previousMats[fairyLights] = fairyLights.materials;
        tempMats = fairyLights.materials;
        tempMats[2] = redGlowMat;
        fairyLights.materials = tempMats;
    }

    public void SetEmergencyState(bool newState)
    {
        shipInEmergencyState = newState;
    }

    public void RevertSecondaryLights()
    {
        if (!shipInEmergencyState) return;

        foreach (Light l in doorSpotLights) l.color = previousColors[l];
        foreach (Light l in ambientLights) l.color = previousColors[l];
        foreach (Light l in spotLights)
        {
            l.color = previousColors[l];
            l.enabled = previousSpotLightState[l];
        }
        foreach (Renderer r in doorSigns) r.materials = previousMats[r];
        foreach (Renderer r in emergencyLights) r.materials = previousMats[r];
        fairyLights.materials = previousMats[fairyLights];
    }
}
