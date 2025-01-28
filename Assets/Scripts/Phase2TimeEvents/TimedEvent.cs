using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class TimedEvent : MonoBehaviour
{
    [Range(0, 59)]
    public int eventMinute;
    [Range(0, 59)]
    public int eventSecond;
    public bool hasBeenTriggered = false;
    private AudioSource audioSource;


    public virtual void TriggerEvent(){
        Debug.Log("Event is triggered.");
        if(audioSource != null)
        {
            audioSource.Play();
            if(audioSource.isPlaying)
            {
                Debug.Log("Audio is playing");
            }
        }
        hasBeenTriggered = true;
    } 
    public virtual bool ShouldEventTrigger(){
        return !hasBeenTriggered && TaskManager.Instance.isPhaseTwo && TimeController.Instance.TimeHasPassed(eventMinute, eventSecond);
    }
    
    public void SetEventStartTime(int min, int sec)
    {
        eventMinute = min;
        eventSecond = sec;
    }
}
