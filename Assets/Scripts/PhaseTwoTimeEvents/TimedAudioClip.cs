using System;
using UnityEngine;

public class TimedAudioClip : TimedEvent
{
    private AudioSource audioSource;
    private void Awake()
    {

    }
    public override void TriggerEvent()
    {
        if (audioSource!= null)
        {
            audioSource.Play();
        }
        TimeController.Instance.radioMessageTimer = 0f;
        TimeController.Instance.radioMessagesPlayed ++;
        base.TriggerEvent();
    } 
}