using System;
using UnityEngine;

public class RapierSixPhaseTwoCommand : TimedEvent
{
    private void Awake()
    {

    }

    public override bool ShouldEventTrigger(){
        return !hasBeenTriggered && TaskManager.Instance.isPhaseTwo && TimeController.Instance.RadioMessageTimeHasPassed(eventMinute, eventSecond);
    }
    public override void TriggerEvent()
    {
        TimeController.Instance.radioMessageTimer = 0f;
        TimeController.Instance.radioMessagesPlayed ++;
        base.TriggerEvent();
    } 
}