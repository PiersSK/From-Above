using System;
using UnityEngine;

public abstract class TimedEvent : MonoBehaviour
{
    [Range(0, 59)]
    public int eventMinute;
    [Range(0, 59)]
    public int eventSecond;
    public bool hasBeenTriggered = false;
    private void Awake()
    {

    }

    public virtual void TriggerEvent()
    {
        hasBeenTriggered = true;
    } 
    public virtual bool ShouldEventTrigger(){
        return !hasBeenTriggered && TimeController.Instance.TimeHasPassed(eventMinute, eventSecond);
    }
    
    public virtual void SetEventStartTime(int min, int sec)
    {
        eventMinute = min;
        eventSecond = sec;
    }
}