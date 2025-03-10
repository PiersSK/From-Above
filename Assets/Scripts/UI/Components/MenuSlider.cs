using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuSlider : MonoBehaviour
{
    [SerializeField] protected Slider slider;
    [SerializeField] protected TextMeshProUGUI valueDisplay;

    [Header("Scaling Options")]
    [Range(-30f, 30f)]
    [SerializeField] protected float actualValueOffset;
    [Range(0f, 30f)]
    [SerializeField] protected float actualValuePerIncrement;

    private void Update()
    {
        valueDisplay.text = slider.value.ToString();
    }
}
