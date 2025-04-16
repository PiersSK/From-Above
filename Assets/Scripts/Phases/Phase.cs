using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "Phase", menuName = "Scriptable Objects/Phase")]
public class Phase : ScriptableObject
{
    [SerializeField] public AudioClip taskBeep;
    [SerializeField] public GameObject taskPad;
    [SerializeField] public TextMeshProUGUI taskPadTrigger;
    [SerializeField] public TextMeshProUGUI taskCount;
    [SerializeField] public TextMeshProUGUI timer;
    [SerializeField] public List<GameObject> taskBlocks;
}
