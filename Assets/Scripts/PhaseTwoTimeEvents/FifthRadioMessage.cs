using System;
using UnityEngine;

public class FifthRadioMessage : TimedEvent
{
    public override bool ShouldEventTrigger()
    {
        return !hasBeenTriggered  && TimeController.Instance.RadioMessageTimeHasPassed(eventMinute, eventSecond) && TimeController.Instance.radioMessagesPlayed == 4 && TaskManager.Instance.phaseTwoTasksCompleted >= 4
        || !hasBeenTriggered && TimeController.Instance.getOnWithItTimer >= 90f && TimeController.Instance.getOnWithItMessagePlayed;
    }
}