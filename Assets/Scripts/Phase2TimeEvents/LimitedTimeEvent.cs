using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedTimedEvent : TimedEvent
{
    [Range(0, 59)]
    public int eventEndMinute;
    [Range(0, 59)]
    public int eventEndSecond;

    public bool eventHasEnded = false;

    public virtual void TriggerEventEnd() { }
    public virtual bool ShouldEventEndTrigger()
    {
        return hasBeenTriggered && !eventHasEnded && TimeController.Instance.TimeHasPassed(eventEndMinute, eventEndSecond);
    }

    public void SetEventEndTime(int min, int sec)
    {
        eventEndMinute = min;
        eventEndSecond = sec;
    }
}