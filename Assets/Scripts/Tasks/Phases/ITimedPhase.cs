using System;

public abstract class TimedPhase : IPhase
{
  public float phaseTimer = 0;
  public int timeLimitMins = 0;

  protected TimeSpan GetRemainingTime()
  {
    return TimeSpan.FromSeconds(timeLimitMins * 60 - TimeController.Instance.GetTimeInSeconds(phaseTimer));
  } 
}