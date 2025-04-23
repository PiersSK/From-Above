using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering;
using TMPro;

public class DailyTaskPad : ITaskPad
{
    [SerializeField] public TextMeshProUGUI taskPadHeader;
    [SerializeField] private List<GameObject> phaseCanvases;
    [SerializeField] private List<Phase> phaseData;

    private int currentPhaseIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"Phase Canvas count {phaseCanvases.Count}");
    }

    protected override void completeTask()
    {
        throw new NotImplementedException();
    }

    protected override void updateTaskPadUI()
    {
        throw new NotImplementedException();
    }
}
