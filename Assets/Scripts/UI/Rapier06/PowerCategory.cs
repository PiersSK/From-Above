using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static System.TimeZoneInfo;

public class PowerCategory : MonoBehaviour
{
    [SerializeField] private List<PowerIcon> batteries = new();
    [SerializeField] private TextMeshProUGUI textLevel;
    [Range(0,3)]
    public int powerLevel = 2;

    protected virtual void Start()
    {
        foreach (PowerIcon b in batteries) b.SetAllocatedState(false);

        for (int i = 0; i < powerLevel; i++)
        {
            batteries[i].SetAllocatedState(true);
        }
        UpdateUIState();
    }

    protected void UpdateUIState()
    {
        textLevel.text = powerLevel.ToString();
    }

    public virtual void LowerPowerLevel(bool overrideTransitionTime = false)
    {
        float transitionTime = overrideTransitionTime ? 0f : PowerRouterController.Instance.powerChangeTransitionTime;

        if (powerLevel > 0)
        {
            batteries[powerLevel - 1].SetAllocatedState(false, overrideTransitionTime);
            if (overrideTransitionTime) PowerMinusOne();
            else Invoke("PowerMinusOne", transitionTime);
        }
    }

    public virtual void IncreasePowerLevel(bool overrideTransitionTime = false)
    {
        float transitionTime = overrideTransitionTime ? 0f : PowerRouterController.Instance.powerChangeTransitionTime;

        if (powerLevel < 3)
        {
            batteries[powerLevel].SetAllocatedState(true, overrideTransitionTime);
            if (overrideTransitionTime) PowerAddOne();
            else Invoke("PowerAddOne", transitionTime);

        }
    }

    private void PowerAddOne() {
        powerLevel++;
        UpdateUIState();
        CommitPowerLevelChange();
    }

    private void PowerMinusOne() {
        powerLevel--;
        UpdateUIState();
        CommitPowerLevelChange();
    }
    protected virtual void CommitPowerLevelChange() { }
}
