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

    private void Start()
    {
        onMat = new(Resources.Load<Material>(ONMATPATH));
        offMat = new(Resources.Load<Material>(OFFMATPATH));
        onMat.color = lightColor;
        onMat.SetColor("_EmissionColor", lightColor * emissiveValue);
        onMat.EnableKeyword("_EMISSION");

        Material mat = lightIsOn ? onMat : offMat;
        foreach(Renderer ren in lightRenderers) ren.material = mat;
        foreach (Light light in lightSources) light.enabled = lightIsOn;
    }

    public void ToggleLight()
    {
        if (!lightStateCanBeChanged) return;

        lightIsOn = !lightIsOn;
        Material mat = lightIsOn ? onMat : offMat;
        foreach (Renderer ren in lightRenderers) ren.material = mat;
        foreach (Light light in lightSources) light.enabled = lightIsOn;
    }
}
