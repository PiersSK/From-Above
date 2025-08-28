using System.Collections.Generic;
using UnityEngine;

public class ShipLight : MonoBehaviour
{
    private const string OFFMATPATH = "LightOff";
    private const string ONMATPATH = "LightOn";

    [SerializeField] private List<Renderer> lightRenderers;
    [SerializeField] private List<Light> lightSources;

    [SerializeField] private bool lightIsOn = false;

    private void Start()
    {
        string mat = lightIsOn ? ONMATPATH : OFFMATPATH;
        foreach(Renderer ren in lightRenderers) ren.material = Resources.Load<Material>(mat);
        foreach (Light light in lightSources) light.enabled = lightIsOn;
    }

    public void ToggleLight()
    {
        lightIsOn = !lightIsOn;
        string mat = lightIsOn ? ONMATPATH : OFFMATPATH;
        foreach (Renderer ren in lightRenderers) ren.material = Resources.Load<Material>(mat);
        foreach (Light light in lightSources) light.enabled = lightIsOn;
    }
}
