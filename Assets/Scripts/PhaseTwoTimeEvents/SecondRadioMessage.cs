using System;
using Unity.VisualScripting;
using UnityEngine;

public class SecondRadioMessage : TimedEvent
{
    public override bool ShouldEventTrigger()
    {
        return (!hasBeenTriggered && TaskManager.Instance.isPhaseTwo && TimeController.Instance.RadioMessageTimeHasPassed(eventMinute, eventSecond)) ||
        (!hasBeenTriggered && TaskManager.Instance.isPhaseTwo && TaskManager.Instance.phaseTwoTasksCompleted == 1);
    }
}