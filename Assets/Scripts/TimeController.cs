using System;
using System.Collections.Generic;
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
    public int radioMessagesPlayed = 0;
    public bool getOnWithItMessagePlayed = false;

    [Header("Time Settings")]
    [Range(0,20)]
    public int phase1TimeLimitMins = 10;
    [Range(0, 20)]
    public int phase2TimeLimitMins = 10;

    [Header("Events")]
    public Transform scheduledEvents;
    public List<string> completeEvents;

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
            if(!isTimePaused)
            {
                radioMessageTimer += Time.deltaTime;
            }
        }
        if (isGetOnWithItTimerSet)
        {
            if(!isGetOnWithItTimerPaused)
            {
                getOnWithItTimer += Time.deltaTime;
            }
        }

        Debug.Log("Time: " + time);
        Debug.Log("Radio Message Timer: " + radioMessageTimer);
        Debug.Log("Get On With It Timer: "+ getOnWithItTimer);
    }

    private void OnEnable()
    {
        VoiceLineManager.VoiceLinePlayed += VoiceLinePlayed;
    }

    private void OnDisable()
    {
        VoiceLineManager.VoiceLinePlayed -= VoiceLinePlayed;
    }

    public float GetTimeInSeconds(float timer)
    {
        return timer;
    }

    public bool TimeHasPassed(float timer,int mins, int secs)
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

    private void VoiceLinePlayed(VoiceLine vl)
    {
        radioMessageTimer = 0f;

        if (vl.name == "Rapier 6 2 - Weapon Warmup")
            isGetOnWithItTimerSet = true;
        
        if (vl.name == "Rapier 6 3 - Pick up the pace" && TaskManager.Instance.phaseTwoTasksCompleted == 3)
            getOnWithItTimer = 0f;

        if (vl.name == "Rapier 6 4 - You are the weapon")
            isGetOnWithItTimerPaused = true;
    }
}