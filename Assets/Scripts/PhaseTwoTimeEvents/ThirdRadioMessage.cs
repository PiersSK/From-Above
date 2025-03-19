using System;
using UnityEngine;

public class ThirdRadioMessage : RapierSixPhaseTwoCommand
{
    public override bool ShouldEventTrigger()
    {
        return (!hasBeenTriggered && TaskManager.Instance.isPhaseTwo && TimeController.Instance.TimeHasPassed(TimeController.Instance.radioMessageTimer, eventMinute, eventSecond)) 
        || (!hasBeenTriggered && TaskManager.Instance.isPhaseTwo && TaskManager.Instance.phaseTwoTasksCompleted == 3);
    }

    public override void TriggerEvent()
    {
        if(TaskManager.Instance.phaseTwoTasksCompleted >= 3)
        {
            TimeController.Instance.getOnWithItTimer = 0;
        }
        TimeController.Instance.radioMessagesPlayed ++;
        base.TriggerEvent();
    }
}