using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;

public class Radio : Interactable
{
    [SerializeField] private Transform needle;
    [SerializeField] private TextMeshProUGUI frequency;

    private int currentFreq = 1;
    private List<float> freqs = new(){69f, 145f, 222f, 298f, 358f};
    [SerializeField] private float maxFrequency = 400f;

    protected override void Interact(Transform player)
    {
        currentFreq++;
        if (currentFreq >= freqs.Count) currentFreq = 0;

        float needlePos = freqs[currentFreq] / maxFrequency * 1.2f;
        needle.localPosition = new Vector3(0f, 1.44f, 0.6f - needlePos);

        frequency.text = freqs[currentFreq] + "Hz";
    }

}
