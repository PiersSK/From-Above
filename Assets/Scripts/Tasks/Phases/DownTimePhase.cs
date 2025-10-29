using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class DownTimePhase : TimedPhase
{

  [Header("Canvas Items")]
  [SerializeField] private TextMeshProUGUI timer;

  public void Update()
  {
    var time = GetRemainingTime();
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