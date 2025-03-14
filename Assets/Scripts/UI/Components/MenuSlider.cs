using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class MenuSlider : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] protected Slider slider;
    [SerializeField] protected TextMeshProUGUI valueDisplay;
    [SerializeField] protected TextMeshProUGUI sliderTitle;
    [SerializeField] protected GameObject selectorChevron;

    [Header("Scaling Options")]
    [Range(-30f, 30f)]
    [SerializeField] protected float actualValueOffset;
    [Range(0f, 30f)]
    [SerializeField] protected float actualValuePerIncrement;

    private void Update()
    {
        valueDisplay.text = slider.value.ToString();
        sliderTitle.color = slider.gameObject == UIManager.Instance.GetSelectedUIObject() ? UIColors.white : UIColors.terminalGreen;
        valueDisplay.color = slider.gameObject == UIManager.Instance.GetSelectedUIObject() ? UIColors.white : UIColors.terminalGreen;
        if (InputManager.Instance.GamepadIsCurrentInput())
            selectorChevron.SetActive(slider.gameObject == UIManager.Instance.GetSelectedUIObject());
        else
            selectorChevron.SetActive(false);
    }

    public float GetModifiedValue()
    {
        return actualValueOffset + actualValuePerIncrement * slider.value;
    }
}
