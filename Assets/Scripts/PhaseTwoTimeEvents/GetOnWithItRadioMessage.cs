using System;
using UnityEngine;

public class GetOnWithItRadioMessage : RapierSixPhaseTwoCommand
{
    public override bool ShouldEventTrigger()
    {
        return !hasBeenTriggered && TimeController.Instance.getOnWithItTimer >= 240f;
    }
    public override void TriggerEvent()
    {
        TimeController.Instance.radioMessageTimer = 0f;
        TimeController.Instance.isTimePaused = true;
        TimeController.Instance.getOnWithItTimer = 0f;
        TimeController.Instance.getOnWithItMessagePlayed = true;
        base.TriggerEvent();
    } 
}