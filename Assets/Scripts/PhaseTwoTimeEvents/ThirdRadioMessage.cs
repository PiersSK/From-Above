using System;
using UnityEngine;

public class ThirdRadioMessage : TimedEvent
{
    public override bool ShouldEventTrigger()
    {
        return (!hasBeenTriggered && TaskManager.Instance.isPhaseTwo && TimeController.Instance.TimeHasPassed(eventMinute, eventSecond)) 
        || (!hasBeenTriggered && TaskManager.Instance.isPhaseTwo && TaskManager.Instance.phaseTwoTasksCompleted == 3);
    }

    public override void TriggerEvent()
    {
        if(TaskManager.Instance.phaseTwoTasksCompleted >= 3)
        {
            TimeController.Instance.getOnWithItTimer = 0;
        }
        base.TriggerEvent();
    }
}