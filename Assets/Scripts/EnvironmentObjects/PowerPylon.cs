using System.Collections;
using UnityEngine;

public class PowerPylon : MonoBehaviour
{
    [SerializeField] private Renderer coilRenderer;
    [SerializeField] private Material blueGlowMaterial;
    [SerializeField] private Material redGlowMaterial;

    [SerializeField] private float cycleDuration = 15f;
    [SerializeField] private float cycleRampupTime = 3f;
    [SerializeField] private float maxCycleEmission = 7f;
    [SerializeField] private float maxWarmupEmission = 5f;
    [SerializeField] private float emissionFlicker = 1f;

    [Header("It's Spreading Easter Egg")]
    [SerializeField] private DataDrive itsSpreadingDrive;
    private bool easterEggActive = false;
    private MusicPlayer mp;

    private void Update()
    {
        if(easterEggActive && mp != null)
        {
            easterEggActive = mp.audioSource.isPlaying;
        }
    }

    private void OnEnable()
    {
        Interactable.PlayerInteracted += ButtonPressed;
    }

    private void ButtonPressed(Interactable interactable)
    {
        if(interactable is CyclePowerButton)
        {
            StopAllCoroutines();
            StartCoroutine(CyclePower());
        } else if (interactable is WarmupButton)
        {
            StopAllCoroutines();
            StartCoroutine(WarmupCoil());
        } else if (interactable is MusicPlayer)
        {
            mp = (MusicPlayer)interactable;
            easterEggActive = mp.dataReader.insertedDrive == itsSpreadingDrive && mp.audioSource.isPlaying;
        }
    }

    private IEnumerator CyclePower()
    {
        Material[] mats = coilRenderer.materials;
        Material glowMat = new(TaskManager.Instance.currentPhase is WeaponTaskPhase ? redGlowMaterial : blueGlowMaterial);
        Material nonglowMat = mats[1];
        Color glowColour = glowMat.color;
        mats[1] = glowMat;
        coilRenderer.materials = mats;

        float elapsed = 0f;
        float i;

        while (elapsed < cycleDuration)
        {
            elapsed += Time.deltaTime;

            if (elapsed < cycleRampupTime) i = elapsed / cycleRampupTime;
            else if (elapsed > (cycleDuration - cycleRampupTime)) i = 1 - ((elapsed - (cycleDuration - cycleRampupTime)) / cycleRampupTime);
            else i = 1;

            i *= maxCycleEmission;
            i += Random.Range(-i/2, i/2);
            i = Mathf.Clamp(i, 0, maxCycleEmission);

            if(easterEggActive) glowColour = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.8f, 1f);

            glowMat.SetColor("_EmissionColor", glowColour * (i * maxCycleEmission));
            glowMat.EnableKeyword("_EMISSION");

            yield return null;
        }

        mats[1] = nonglowMat;
        coilRenderer.materials = mats;
    }

    private IEnumerator WarmupCoil()
    {
        Material[] mats = coilRenderer.materials;
        Material glowMat = new(redGlowMaterial);
        Color glowColour = glowMat.color;
        mats[1] = glowMat;
        coilRenderer.materials = mats;

        float elapsed = 0f;
        float i;

        while (elapsed < cycleDuration)
        {
            elapsed += Time.deltaTime;
            i = elapsed / cycleDuration;
            i *= maxWarmupEmission;
            glowMat.SetColor("_EmissionColor", glowColour * i);
            glowMat.EnableKeyword("_EMISSION");

            yield return null;
        }

        glowMat.SetColor("_EmissionColor", glowColour * maxWarmupEmission);
        glowMat.EnableKeyword("_EMISSION");
    }
}
