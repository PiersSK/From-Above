using System;
using UnityEngine;

public class FifthRadioMessage : RapierSixPhaseTwoCommand
{
    public override bool ShouldEventTrigger()
    {
        return !hasBeenTriggered  && TimeController.Instance.TimeHasPassed(TimeController.Instance.radioMessageTimer, eventMinute, eventSecond)
        && TimeController.Instance.radioMessagesPlayed == 4 && TaskManager.Instance.phaseTwoTasksCompleted >= 4
        || !hasBeenTriggered && TimeController.Instance.getOnWithItTimer >= 180f && TimeController.Instance.getOnWithItMessagePlayed;
    }
}