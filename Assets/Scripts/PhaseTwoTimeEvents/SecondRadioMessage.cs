using System;
using Unity.VisualScripting;
using UnityEngine;

public class SecondRadioMessage : RapierSixPhaseTwoCommand
{
    public override bool ShouldEventTrigger()
    {
        return (!hasBeenTriggered && TaskManager.Instance.isPhaseTwo && TimeController.Instance.TimeHasPassed(TimeController.Instance.radioMessageTimer, eventMinute, eventSecond)) ||
        (!hasBeenTriggered && TaskManager.Instance.isPhaseTwo && TaskManager.Instance.phaseTwoTasksCompleted == 1);
    }
    public override void TriggerEvent()
    {
        TimeController.Instance.radioMessagesPlayed ++;
        base.TriggerEvent();
    }
}