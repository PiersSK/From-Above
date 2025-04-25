using System;
using UnityEngine;

public class ThirdRadioMessage : TimedEvent
{
    public override bool ShouldEventTrigger()
    {
        return (!hasBeenTriggered && TaskManager.Instance.isWeaponPhase && TimeController.Instance.RadioMessageTimeHasPassed(eventMinute, eventSecond)) 
        || (!hasBeenTriggered && TaskManager.Instance.isWeaponPhase && TaskManager.Instance.weaponPhaseCompletedTasks == 3);
    }

    public override void TriggerEvent()
    {
        if(TaskManager.Instance.weaponPhaseCompletedTasks >= 3)
        {
            TimeController.Instance.getOnWithItTimer = 0;
        }
        base.TriggerEvent();
    }
}