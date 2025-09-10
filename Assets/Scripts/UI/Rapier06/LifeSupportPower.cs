using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LifeSupportPower : PowerCategory
{
    [SerializeField] private AudioSource fanSource;
    [SerializeField] private Image deathScreen;
    [SerializeField] private float timeToDie = 10f;
    private float fanDefaultVol;
    private float playerBaseDefault;
    private float playerSprintDefault;

    protected override void Start()
    {
        base.Start();
        fanDefaultVol = fanSource.volume;
        playerBaseDefault = PlayerMotor.Instance.baseSpeed;
        playerSprintDefault = PlayerMotor.Instance.sprintSpeed;
    }

    // Base + Sprint are %, Fan is +/-
    private void ChangeLifeSupportState(float baseMod, float sprintMod, float fanMod)
    {
        PlayerMotor.Instance.baseSpeed = Mathf.Clamp(playerBaseDefault * baseMod, 0, Mathf.Infinity);
        PlayerMotor.Instance.sprintSpeed = Mathf.Clamp(playerSprintDefault * sprintMod, PlayerMotor.Instance.baseSpeed, Mathf.Infinity);
        PlayerMotor.Instance.currentSpeed = PlayerMotor.Instance.sprinting ? PlayerMotor.Instance.sprintSpeed : PlayerMotor.Instance.baseSpeed;
        fanSource.volume = fanDefaultVol + fanMod;
    }

    protected override void CommitPowerLevelChange()
    {
        base.CommitPowerLevelChange();
        switch(powerLevel)
        {
            case 0:
                StartCoroutine(KillPlayer());
                ChangeLifeSupportState(0.3f, 0f, -1f);
                break;
            case 1:
                ChangeLifeSupportState(0.8f, 0.7f, -0.15f);
                break;
            case 2:
                ChangeLifeSupportState(1f, 1f, 0f);
                break;
            case 3:
                ChangeLifeSupportState(1.2f, 1.3f, 0.15f);
                break;
            default:
                Debug.LogError("LifeSupportPower asked to update to impossible power level");
                break;
        }
    }


    // TODO: Add camera sway
    // TODO: Add gasping for breath SFX
    // TODO: Slow ladder climb speed
    private IEnumerator KillPlayer()
    {
        float time = 0f;
        AudioLowPassFilter alp = Camera.main.GetComponent<AudioLowPassFilter>();
        AudioReverbFilter arf = Camera.main.GetComponent<AudioReverbFilter>();
        arf.enabled = true;
        alp.enabled = true;

        deathScreen.gameObject.SetActive(true);

        while(time < timeToDie && powerLevel == 0)
        {
            time += Time.deltaTime;
            float p = time / timeToDie;

            float x = p * Mathf.Abs(Mathf.Sin(p * 2.5f * Mathf.PI));
            deathScreen.color = new Color(0, 0, 0, x);
            alp.cutoffFrequency = 1000f * (1 - p);

            yield return null;
        }

        if (powerLevel == 0)
        {
            deathScreen.transform.GetChild(0).gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            deathScreen.gameObject.SetActive(false);
            deathScreen.color = new Color(1, 1, 1, 0f);
            alp.enabled = false;
            alp.cutoffFrequency = 1000f;
            arf.enabled = false;
        }
    }
}
