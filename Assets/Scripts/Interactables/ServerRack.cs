using TMPro;
using UnityEngine;

public class ServerRack : MonoBehaviour
{
    [SerializeField] private Renderer leftLight;
    [SerializeField] private Renderer rightLight;
    public bool hasDisk = false;
    [SerializeField] private bool isUsable = false;

    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private Renderer labelBG;
    [SerializeField] private Light spotlight;
    [SerializeField] private string customLabel;
    [SerializeField] private bool useCustomLabel = false;
    [SerializeField] private bool spotLightOn = false;

    private const string LEDGreen = "LEDGreen";
    private const string LEDBlue = "LEDBlue";
    private const string LEDRed = "LEDRed";
    private const string DimGlow = "DimGlow";
    private const string WhiteGlow = "WhiteGlow";

    private void Update()
    {
        SetLights();
    }

    public void SetLights()
    {
        bool diskPresent = GetComponentInChildren<ServerDiscStorage>().driveInDock;
        leftLight.material = Resources.Load<Material>(hasDisk && diskPresent ? LEDGreen : LEDRed);

        bool inPhase = GetComponentInChildren<ServerButton>().usableInPhaseOne || TaskManager.Instance.isPhaseTwo;
        rightLight.material = Resources.Load<Material>(isUsable && !GetComponentInChildren<ServerButton>().exe.hasRun && inPhase ? LEDBlue : LEDRed);

        spotlight.enabled = spotLightOn;
    }

    public void SetLabel(string name)
    {
        label.text = useCustomLabel ? customLabel : name;
        labelBG.material = Resources.Load<Material>(useCustomLabel ? WhiteGlow : DimGlow);
    }
}
