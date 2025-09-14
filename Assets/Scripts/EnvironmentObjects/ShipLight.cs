using System.Collections.Generic;
using UnityEngine;

public class ShipLight : MonoBehaviour
{
    private const string OFFMATPATH = "LightOff";
    private const string ONMATPATH = "LightOn";
    private Material onMat;
    private Material offMat;

    [SerializeField] private List<Renderer> lightRenderers;
    [SerializeField] private List<Light> lightSources;
    [SerializeField] private Color lightColor = UIColors.white;
    [SerializeField] private float emissiveValue = 3.5f;

    public bool lightIsOn = false;
    public bool lightStateCanBeChanged = true;

    private bool previousOnState;
    private bool previousChangeableState;
    private Dictionary<Light, float> startIntensities = new();

    private void Start()
    {
        onMat = new(Resources.Load<Material>(ONMATPATH));
        offMat = new(Resources.Load<Material>(OFFMATPATH));
        onMat.color = lightColor;
        onMat.SetColor("_EmissionColor", lightColor * emissiveValue);
        onMat.EnableKeyword("_EMISSION");

        Material mat = lightIsOn ? onMat : offMat;
        foreach(Renderer ren in lightRenderers) ren.material = mat;
        foreach (Light light in lightSources) {
            light.enabled = lightIsOn;
            startIntensities[light] = light.intensity;
        }

        previousOnState = lightIsOn;
        previousChangeableState = lightStateCanBeChanged;
    }

    public void SetLightState(bool setToOn)
    {
        previousOnState = lightIsOn;

        lightIsOn = setToOn;
        Material mat = lightIsOn ? onMat : offMat;
        foreach (Renderer ren in lightRenderers) ren.material = mat;
        foreach (Light light in lightSources) light.enabled = lightIsOn;
    }

    public void SetLightToBright()
    {
        foreach(Light l in lightSources) l.intensity = startIntensities[l] * 2f;
    }

    public void SetLightToStartIntensity()
    {
        foreach (Light l in lightSources) l.intensity = startIntensities[l];
    }

    public void SetChangeableState(bool canBeChanged)
    {
        previousChangeableState = lightStateCanBeChanged;
        lightStateCanBeChanged = canBeChanged;
    }

    public void RevertToPreviousState()
    {
        SetChangeableState(previousChangeableState);
        SetLightState(previousOnState);
    }

    public void ToggleLight()
    {
        if (!lightStateCanBeChanged) return;

        SetLightState(!lightIsOn);
    }
}
