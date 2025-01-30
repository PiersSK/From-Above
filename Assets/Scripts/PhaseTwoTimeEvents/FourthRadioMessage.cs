using System;
using UnityEngine;

public class FourthRadioMessage : TimedEvent
{
     public override bool ShouldEventTrigger()
    {
        return (!hasBeenTriggered && TaskManager.Instance.isPhaseTwo && TimeController.Instance.RadioMessageTimeHasPassed(eventMinute, eventSecond)) ||
        (!hasBeenTriggered && TaskManager.Instance.isPhaseTwo && TaskManager.Instance.phaseTwoTasksCompleted == 4);
    }
    public override void TriggerEvent()
    {
        if(TaskManager.Instance.phaseTwoTasksCompleted == 4)
        {
            TimeController.Instance.isGetOnWithItTimerPaused = true;
        }
        base.TriggerEvent();
    }
}