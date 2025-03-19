using System;
using UnityEngine;

public class FirstRadioMessage : RapierSixPhaseTwoCommand
{
    public override bool ShouldEventTrigger(){
        return !hasBeenTriggered && TaskManager.Instance.isPhaseTwo && TimeController.Instance.TimeHasPassed(TimeController.Instance.radioMessageTimer, eventMinute, eventSecond);
    }
    public override void TriggerEvent()
    {
        TimeController.Instance.radioMessagesPlayed ++;
        base.TriggerEvent();
    }
}