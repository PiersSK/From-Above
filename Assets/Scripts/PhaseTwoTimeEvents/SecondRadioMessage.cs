using System;
using Unity.VisualScripting;
using UnityEngine;

public class SecondRadioMessage : TimedEvent
{
    public override bool ShouldEventTrigger()
    {
        return (!hasBeenTriggered && TaskManager.Instance.isWeaponPhase && TimeController.Instance.RadioMessageTimeHasPassed(eventMinute, eventSecond)) ||
        (!hasBeenTriggered && TaskManager.Instance.isWeaponPhase && TaskManager.Instance.weaponPhaseCompletedTasks == 1);
    }
}