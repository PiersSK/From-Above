using System;
using UnityEngine;

public class FourthRadioMessage : RapierSixPhaseTwoCommand
{
     public override bool ShouldEventTrigger()
    {
        return (!hasBeenTriggered && TaskManager.Instance.isPhaseTwo
        && TimeController.Instance.TimeHasPassed(TimeController.Instance.radioMessageTimer,eventMinute, eventSecond)) ||
        (!hasBeenTriggered && TaskManager.Instance.isPhaseTwo && TaskManager.Instance.phaseTwoTasksCompleted == 4);
    }
    public override void TriggerEvent()
    {
        if(TaskManager.Instance.phaseTwoTasksCompleted == 4)
        {
            TimeController.Instance.isGetOnWithItTimerPaused = true;
            TimeController.Instance.isTimePaused = false;
        }
        base.TriggerEvent();
    }
}