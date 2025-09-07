using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class DownTimePhase : IPhase
{

  [Header("Canvas Items")]
  [SerializeField] private TextMeshProUGUI taskCount;
  [SerializeField] private TextMeshProUGUI timer;

  public void Update()
  {
    TimeSpan time = TimeSpan.FromSeconds(TimeController.Instance.downTimePhsaeLimitMins * 60 - TimeController.Instance.GetTimeInSeconds(TimeController.Instance.time));
    timer.text = $"{time.Minutes:00}:{time.Seconds:00}";
  }
  public override void BeginCurrentPhase()
  {
    base.BeginCurrentPhase();
    UpdateTaskPadUI();
  }

  public override void UpdateTaskPadUI()
  {
    base.UpdateTaskPadUI();
  }

  public override void CompleteTask(TaskData completedTask)
  {
    base.CompleteTask(completedTask);
  }

  public override void EndCurrentPhase()
  {
    base.EndCurrentPhase();
  }
}