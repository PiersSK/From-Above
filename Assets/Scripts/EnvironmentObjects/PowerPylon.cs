using System.Collections;
using UnityEngine;

public class PowerPylon : MonoBehaviour
{
    [SerializeField] private Renderer coilRenderer;
    [SerializeField] private Material blueGlowMaterial;
    [SerializeField] private Material redGlowMaterial;

    [SerializeField] private float cycleDuration = 15f;
    [SerializeField] private float cycleRampupTime = 3f;
    [SerializeField] private float maxEmission = 7f;
    [SerializeField] private float emissionFlicker = 1f;

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
        }
    }

    private IEnumerator CyclePower()
    {
        Material[] mats = coilRenderer.materials;
        Material glowMat = new(blueGlowMaterial);
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

            i += Random.Range(-emissionFlicker, emissionFlicker);
            i = Mathf.Clamp(i, 0, 1);
            glowMat.SetColor("_EmissionColor", glowColour * (i * maxEmission));
            glowMat.EnableKeyword("_EMISSION");
            //mats[1] = glowMat;
            //coilRenderer.materials = mats;
            yield return null;
        }

        mats[1] = nonglowMat;
        coilRenderer.materials = mats;
    }
}
