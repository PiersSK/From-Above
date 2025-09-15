using UnityEngine;

public class PowerRouterController : MonoBehaviour
{
    public static PowerRouterController Instance;
    public PowerCategory lifePower;
    public PowerCategory shipPower;
    public PowerCategory rapierPower;

    public GameObject lowPowerStateScreen;
    public Animation lowPowerAnimation;

    public float powerChangeTransitionTime = 3f;

    public bool onCooldown = false;

    private void Awake()
    {
        Instance = this;
    }

    public void SetToLowPower()
    {
        lowPowerAnimation.Play();
    }

    public void SetToNormalPower()
    {
        lowPowerStateScreen.SetActive(false);
    }

    //Warning: When used, can override "total power" breaking the game so use carefully
    public void ForcePowerToLevel(PowerCategory cat, int level)
    {
        int i = cat.powerLevel - level;
        for(int j = 0; j < Mathf.Abs(i); j++)
        {
            if (i < 0) cat.IncreasePowerLevel(true);
            else if (i > 0) cat.LowerPowerLevel(true);
        }
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
