using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    public virtual void LowerPowerLevel()
    {
        if (powerLevel > 0)
        {
            batteries[powerLevel - 1].SetAllocatedState(false);
            Invoke("PowerMinusOne", PowerRouterController.Instance.powerChangeTransitionTime);
            Invoke("CommitPowerLevelChange", PowerRouterController.Instance.powerChangeTransitionTime + 0.1f);
        }
    }

    public virtual void IncreasePowerLevel()
    {
        if (powerLevel < 3)
        {
            batteries[powerLevel].SetAllocatedState(true);
            Invoke("PowerAddOne", PowerRouterController.Instance.powerChangeTransitionTime);
            Invoke("CommitPowerLevelChange", PowerRouterController.Instance.powerChangeTransitionTime + 0.1f);

        }
    }

    private void PowerAddOne() {
        powerLevel++;
        UpdateUIState();
    }

    private void PowerMinusOne() {
        powerLevel--;
        UpdateUIState();
    }
    protected virtual void CommitPowerLevelChange() { }
}
