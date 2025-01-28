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

    [SerializeField] private float broadcastFrequency = 60f;
    public float broadcastTimer = 0f;

    [SerializeField] private List<AudioClip> messages = new List<AudioClip>(5);
    [SerializeField] private List<float> broadcastStarts = new();

    [SerializeField] private AudioSource source;

    private void Start()
    {
        foreach(var freq in freqs)
        {
            broadcastStarts.Add(Random.Range(10f, broadcastFrequency));
        }
    }

    private void Update()
    {
        broadcastTimer += Time.deltaTime;
        
        AudioClip clip = messages[currentFreq];

        if (clip != null)
        {
            float length = clip.length;

            if (broadcastTimer >= broadcastStarts[currentFreq])
            {
                float cycleLength = length + broadcastFrequency; 
                float timerInCycle = (broadcastTimer - broadcastStarts[currentFreq]) % cycleLength;
                if (timerInCycle <= length && !source.isPlaying)
                {
                    source.clip = clip;
                    source.time = timerInCycle;
                    source.Play();
                }
                else if (timerInCycle > length && source.isPlaying)
                {
                    source.Pause();
                }
            }
        }
    }


    protected override void Interact(Transform player)
    {
        currentFreq++;
        if (currentFreq >= freqs.Count) currentFreq = 0;

        float needlePos = freqs[currentFreq] / maxFrequency * 1.2f;
        needle.localPosition = new Vector3(0f, 1.44f, 0.6f - needlePos);

        frequency.text = freqs[currentFreq] + "Hz";
        source.Pause();
    }

}
