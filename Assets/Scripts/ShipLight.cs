using UnityEngine;

public class ShipLight : MonoBehaviour
{
    private const string OFFMATPATH = "LightOff";
    private const string ONMATPATH = "LightOn";

    [SerializeField] private Renderer lightRenderer;
    [SerializeField] private Light lightSource;

    [SerializeField] private bool lightIsOn = false;

    public void ToggleLight()
    {
        lightIsOn = !lightIsOn;
        if (lightIsOn)
        {
            lightRenderer.material = Resources.Load<Material>(ONMATPATH);
            lightSource.enabled = true;
        }
        else
        {
            lightRenderer.material = Resources.Load<Material>(OFFMATPATH);
            lightSource.enabled = false;
        }
    }
}
