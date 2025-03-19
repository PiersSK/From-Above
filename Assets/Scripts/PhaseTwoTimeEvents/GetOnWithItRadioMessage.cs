using System;
using UnityEngine;

public class GetOnWithItRadioMessage : TimedAudioClip
{
    public override bool ShouldEventTrigger()
    {
        return !hasBeenTriggered && TimeController.Instance.getOnWithItTimer >= 240f;
    }
    public override void TriggerEvent()
    {
        TimeController.Instance.radioMessageTimer = 0f;
        TimeController.Instance.radioMessagesPlayed -= 1;
        TimeController.Instance.isTimePaused = true;
        TimeController.Instance.getOnWithItTimer = 0f;
        TimeController.Instance.getOnWithItMessagePlayed = true;
        base.TriggerEvent();
    } 
}