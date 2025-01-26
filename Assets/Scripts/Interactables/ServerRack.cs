using TMPro;
using UnityEngine;

public class ServerRack : MonoBehaviour
{
    [SerializeField] private Renderer leftLight;
    [SerializeField] private Renderer rightLight;
    [SerializeField] private bool hasDisk = false;
    [SerializeField] private bool isUsable = false;

    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private Renderer labelBG;
    [SerializeField] private string customLabel;
    [SerializeField] private bool useCustomLabel = false;

    private const string LEDGreen = "LEDGreen";
    private const string LEDBlue = "LEDBlue";
    private const string LEDRed = "LEDRed";
    private const string DimGlow = "DimGlow";
    private const string WhiteGlow = "WhiteGlow";

    private void Start()
    {
        leftLight.material = Resources.Load<Material>(hasDisk ? LEDGreen : LEDRed);
        rightLight.material = Resources.Load<Material>(isUsable ? LEDBlue : LEDRed);

        if (useCustomLabel) label.text = customLabel;
    }

    public void SetLabel(string name)
    {
        label.text = useCustomLabel ? customLabel : name;
        labelBG.material = Resources.Load<Material>(useCustomLabel ? WhiteGlow : DimGlow);
    }
}
