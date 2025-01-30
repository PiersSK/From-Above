using System;
using UnityEngine;

public class FifthRadioMessage : TimedEvent
{
    public override bool ShouldEventTrigger()
    {
        return !hasBeenTriggered && TaskManager.Instance.isPhaseTwo && TimeController.Instance.TimeHasPassed(eventMinute, eventSecond) && TimeController.Instance.radioMessagesPlayed == 4
        || !hasBeenTriggered && TaskManager.Instance.isPhaseTwo && TimeController.Instance.getOnWithItTimer >= 180f && TimeController.Instance.getOnWithItMessagePlayed;
    }
}