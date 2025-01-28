using System;
using Unity.VisualScripting;
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
        return !hasBeenTriggered && TaskManager.Instance.isPhaseTwo && TimeController.Instance.TimeHasPassed(eventMinute, eventSecond);
    }
    
    public void SetEventStartTime(int min, int sec)
    {
        eventMinute = min;
        eventSecond = sec;
    }
}
