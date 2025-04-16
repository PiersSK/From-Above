using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "Phase", menuName = "Scriptable Objects/Phase")]
public class Phase : ScriptableObject
{
    [SerializeField] public AudioClip taskBeep;
    [SerializeField] public GameObject taskPad;
    [SerializeField] public GameObject taskPadTrigger;
    [SerializeField] public TextMeshProUGUI taskCount;
    [SerializeField] public TextMeshProUGUI timer;
    [SerializeField] public List<Task> tasks;
    [SerializeField] public List<GameObject> taskBlocks;
}

//!!!Notes I am to delete later!!!
//Fun things we've learnt today:
//1. You CANNOT reference an instance of an object from the game scene into a scriptable object
//2. We current use almost EXCLUSIVELY instances of in scene objects in the task manager for phase tracking
// This is not a criticism of the current system, it just means I can't transition my shiney new SO into 
// the task manager as incrementally as I had hoped.
//Time to crack open the crowbars 