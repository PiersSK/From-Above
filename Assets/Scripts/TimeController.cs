using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public static TimeController Instance { get; private set;}
    public float time = 0f;
    public float radioMessageTimer = 0f;
    public float getOnWithItTimer = 0f;
    private bool isTimeSet = false;
    public bool isGetOnWithItTimerSet = false;
    public bool isTimePaused = false;
    public bool isGetOnWithItTimerPaused = false;
    private int currentMin;
    private int currentSec;
    public int radioMessagesPlayed = 0;
    public bool getOnWithItMessagePlayed = false;

    [Header("Time Settings")]
    [Range(0,20)]
    public int phase1TimeLimitMins = 10;
    [Range(0, 20)]
    public int phase2TimeLimitMins = 10;
    private int startTimeMins = 0;

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
        if(!isTimeSet && TaskManager.Instance.isPhaseTwo)
        {
            isTimeSet = true;
        } else if(isTimeSet)
        {
            time += Time.deltaTime;
            TriggerEvents();
            if(!isTimePaused)
            {
                radioMessageTimer += Time.deltaTime;
            }
        }
        if(!isGetOnWithItTimerSet && radioMessagesPlayed >= 2)
        {
            isGetOnWithItTimerSet = true;
        } else if (isGetOnWithItTimerSet)
        {
            if(!isGetOnWithItTimerPaused)
            {
                getOnWithItTimer += Time.deltaTime;
                Debug.Log("Get On With It Timer: " + getOnWithItTimer);
            }
        }
    }

    public float GetTimeInSeconds()
    {
        return time;
    }

    public bool TimeHasPassed(int mins, int secs)
    {
        return time > mins * 60 + secs;
    }

    public bool TimeHasPassed(TimeSpan timeToCompare)
    {
        return CurrentTime() >= timeToCompare;
    }

    public bool RadioMessageTimeHasPassed(int mins, int secs)
    {
        return radioMessageTimer > mins * 60 + secs;
    }
    public bool RadioMessageTimeHasPassed (TimeSpan timeToCompare)
    {
        return CurrentRadioMessageTime() >= timeToCompare;
    }

    public bool GetOnWithItTimePassed(int mins, int secs)
    {
        return getOnWithItTimer > mins * 60 + secs;
    }

    public bool GetOnWithItTimePassed(TimeSpan timeToCompare)
    {
        return CurrentGetOnWithItTime() >= timeToCompare;
    }

    public TimeSpan CurrentTime()
    {
        return TimeSpan.FromSeconds(time);
    }

    public TimeSpan CurrentGetOnWithItTime()
    {
        return TimeSpan.FromSeconds(getOnWithItTimer);
    }
    public TimeSpan CurrentRadioMessageTime()
    {
        return TimeSpan.FromSeconds(getOnWithItTimer);
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
                if(e.ShouldEventTrigger())
                {
                e.TriggerEvent();
                e.hasBeenTriggered = true;
                }
            }
        }
    }
}