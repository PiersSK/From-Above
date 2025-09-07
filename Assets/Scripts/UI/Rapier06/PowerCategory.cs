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
        foreach (PowerIcon b in batteries) b.SetAllocatedState(false);

        for (int i = 0; i < powerLevel; i++)
        {
            batteries[i].SetAllocatedState(true);
        }
        UpdateUIState();
    }

    private void UpdateUIState()
    {
        textLevel.text = powerLevel.ToString();
    }

    public void LowerPowerLevel()
    {
        if (powerLevel > 0)
        {
            powerLevel--;
            batteries[powerLevel].SetAllocatedState(false);
            UpdateUIState();
        }
    }

    public void IncreasePowerLevel()
    {
        if (powerLevel < 3)
        {
            powerLevel++;
            batteries[powerLevel - 1].SetAllocatedState(true);
            UpdateUIState();
        }
    }
}
