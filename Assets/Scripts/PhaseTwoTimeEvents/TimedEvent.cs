using System;
using UnityEngine;

public class TimedEvent : MonoBehaviour
{
    [Range(0, 59)]
    public int eventMinute;
    [Range(0, 59)]
    public int eventSecond;
    public bool hasBeenTriggered = false;
    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public virtual void TriggerEvent()
    {
        if(audioSource != null)
        {
            audioSource.Play();
        }
        hasBeenTriggered = true;
        TimeController.Instance.radioMessageTimer = 0f;
        TimeController.Instance.radioMessagesPlayed ++;
    } 
    public virtual bool ShouldEventTrigger(){
        return !hasBeenTriggered && TaskManager.Instance.isPhaseTwo && TimeController.Instance.RadioMessageTimeHasPassed(eventMinute, eventSecond);
    }
    
    public void SetEventStartTime(int min, int sec)
    {
        eventMinute = min;
        eventSecond = sec;
    }
}