using System;
using UnityEngine;

public class FourthRadioMessage : TimedEvent
{
     public override bool ShouldEventTrigger()
    {
        return (!hasBeenTriggered && TaskManager.Instance.isWeaponPhase && TimeController.Instance.RadioMessageTimeHasPassed(eventMinute, eventSecond)) ||
        (!hasBeenTriggered && TaskManager.Instance.isWeaponPhase && TaskManager.Instance.weaponPhaseCompletedTasks == 4);
    }
    public override void TriggerEvent()
    {
        if(TaskManager.Instance.weaponPhaseCompletedTasks == 4)
        {
            TimeController.Instance.isGetOnWithItTimerPaused = true;
            TimeController.Instance.isTimePaused = false;
        }
        base.TriggerEvent();
    }
}