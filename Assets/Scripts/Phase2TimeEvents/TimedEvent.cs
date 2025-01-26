using System;
using UnityEngine;

public class TimedEvent : MonoBehaviour
{
    [Range(0, 59)]
    public int eventMinute;
    [Range(0, 59)]
    public int eventSecond;

    public bool hasBeenTriggered = false;

    public virtual void TriggerEvent(){} 
    public virtual bool ShouldEventTrigger(){
        return !hasBeenTriggered && TimeController.Instance.hasPhaseTwoStarted && TimeController.Instance.TimeHasPassed(eventMinute, eventSecond);
    }
    
    public void SetEventStartTime(int min, int sec)
    {
        eventMinute = min;
        eventSecond = sec;
    }
}
