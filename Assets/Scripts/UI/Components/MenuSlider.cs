using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI valueDisplay;

    [Header("Scaling Options")]
    [Range(-30f, 30f)]
    [SerializeField] private float actualValueOffset;
    [Range(0f, 30f)]
    [SerializeField] private float actualValuePerIncrement;

    private void Update()
    {
        valueDisplay.text = slider.value.ToString();
    }
}
