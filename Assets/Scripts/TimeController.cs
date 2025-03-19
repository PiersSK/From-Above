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
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
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
            }
        }
    }

    public float GetTimeInSeconds()
    {
        return time;
    }

    public bool TimeHasPassed(float timer, int mins, int secs)
    {
        return timer > mins * 60 + secs;
    }

    public bool TimeHasPassed(float timer, TimeSpan timeToCompare)
    {
        return CurrentTime(timer) >= timeToCompare;
    }


    public TimeSpan CurrentTime(float timer)
    {
        return TimeSpan.FromSeconds(timer);
    }

    public bool IsInTimeSpan(float timer, int min1, int sec1, int min2, int sec2)
    {
        return TimeHasPassed(timer, min1, sec1) && !TimeHasPassed(timer, min2, sec2);
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