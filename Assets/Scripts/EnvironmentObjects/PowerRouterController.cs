using System.Collections;
using UnityEngine;

public class PowerRouterController : MonoBehaviour
{
    public static PowerRouterController Instance;
    [SerializeField] private PowerCategory lifePower;
    [SerializeField] private PowerCategory shipPower;
    [SerializeField] private PowerCategory rapierPower;

    public float powerChangeTransitionTime = 3f;

    public bool onCooldown = false;

    private void Awake()
    {
        Instance = this;
    }

    public void IncreasePowerLevel(PowerCategory cat)
    {
        cat.IncreasePowerLevel();

        if(cat != lifePower && lifePower.powerLevel > 1)
        {
            lifePower.LowerPowerLevel();
        } else if (cat != shipPower && shipPower.powerLevel > 1)
        {
            shipPower.LowerPowerLevel();
        } else if (cat != rapierPower && rapierPower.powerLevel > 1)
        {
            rapierPower.LowerPowerLevel();
        }
    }

    public void LowerPowerLevel(PowerCategory cat)
    {
        cat.LowerPowerLevel();

        if (cat != rapierPower && rapierPower.powerLevel < 2)
        {
            rapierPower.IncreasePowerLevel();
        }
        else if (cat != shipPower && shipPower.powerLevel < 3)
        {
            shipPower.IncreasePowerLevel();
        }
        else if (cat != lifePower && lifePower.powerLevel < 3)
        {
            lifePower.IncreasePowerLevel();
        }
    }
}
