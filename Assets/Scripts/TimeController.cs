using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public static TimeController Instance { get; private set;}

    private int startTimeMins = 0;
    private float time = 0f;
    private int currentMin;
    private int currentSec;
    private bool isTimeSet = false;
    public  bool hasPhaseTwoStarted = false;

    [Header("Events")]
    public Transform scheduledEvents;
    public List<string> completeEvents;

   // private List<TimedEvent> scheduledEvents;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(!isTimeSet && hasPhaseTwoStarted)
        {
            time = startTimeMins * 60;
            isTimeSet = true;
        }
        if(TaskManager.Instance.Tasks.Count == 0)
        {
            hasPhaseTwoStarted = true;
        }

    }

    public bool TimeHasPassed(int mins, int secs)
    {
        return time > mins * 60 + secs;
    }

    public bool TimeHasPassed(TimeSpan timeToCompare)
    {
        return CurrentTime() >= timeToCompare;
    }

    public TimeSpan CurrentTime()
    {
        return TimeSpan.FromSeconds(time);
    }

    public bool IsInTimeSpan(int min1, int sec1, int min2, int sec2)
    {
        return TimeHasPassed(min1, sec1) && !TimeHasPassed(min2, sec2);
    }

    private void TriggerEvents()
    {
        foreach (Transform eventTransform in scheduledEvents)
        {
            if (eventTransform.TryGetComponent(out TimedEvent e))
            {
                if (e.ShouldEventTrigger())
                {
                    e.TriggerEvent();
                    e.hasBeenTriggered = true;
                }
            }

            if (e.TryGetComponent(out LimitedTimedEvent lte))
            {
                if (lte.ShouldEventTrigger())
                {
                    lte.TriggerEventEnd();
                    lte.eventHasEnded = true;
                }
            }
        }
    }


}