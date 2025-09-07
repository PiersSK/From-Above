using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerCategory : MonoBehaviour
{
    [SerializeField] private List<PowerIcon> batteries = new();
    [SerializeField] private TextMeshProUGUI textLevel;
    [Range(0,3)]
    public int powerLevel = 2;

    private void Start()
    {
        UpdateUIState();
    }

    private void UpdateUIState()
    {
        foreach (PowerIcon b in batteries) b.SetAllocatedState(false);

        for(int i = 0; i < powerLevel; i++)
        {
            batteries[i].SetAllocatedState(true);
        }

        textLevel.text = powerLevel.ToString();
    }

    public void LowerPowerLevel()
    {
        powerLevel--;
        powerLevel = Mathf.Clamp(powerLevel, 0, 3);
        UpdateUIState();
    }

    public void IncreasePowerLevel()
    {
        powerLevel++;
        powerLevel = Mathf.Clamp(powerLevel, 0, 3);
        UpdateUIState();
    }
}
