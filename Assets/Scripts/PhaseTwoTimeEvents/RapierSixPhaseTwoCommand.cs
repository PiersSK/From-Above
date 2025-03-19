using System;
using UnityEngine;

public class RapierSixPhaseTwoCommand : TimedEvent
{
    private void Awake()
    {

    }

    public override void TriggerEvent()
    {
        TimeController.Instance.radioMessageTimer = 0f;
        TimeController.Instance.radioMessagesPlayed ++;
        base.TriggerEvent();
    } 
}