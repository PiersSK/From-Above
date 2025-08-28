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

    public void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        shipLights = FindObjectsByType<ShipLight>(FindObjectsSortMode.None);
    }

    public void ShutDownAllShipLights()
    {
        foreach(ShipLight light in shipLights)
        {
            if(light.lightIsOn) light.ToggleLight();
            light.lightStateCanBeChanged = false;
        }
    }

    public void SetLightsRed()
    {
        Material[] tempMats;

        foreach(Light l in doorSpotLights) l.color = Color.red;
        foreach(Light l in ambientLights) l.color = Color.red;
        foreach (Light l in spotLights)
        {
            l.enabled = true;
            l.color = Color.red;
        }
        foreach (Renderer r in doorSigns)
        {
            tempMats = r.materials;
            tempMats[1] = redGlowMat;
            r.materials = tempMats;
        }
        foreach (Renderer r in emergencyLights) r.material = redGlowMat;

        // TODO: This feels very specific to RAPIER06
        tempMats = fairyLights.materials;
        tempMats[2] = redGlowMat;
        fairyLights.materials = tempMats;
    }
}
