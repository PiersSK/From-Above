using System;
using UnityEngine;

public class FirstRadioMessage : TimedEvent
{
    public override void TriggerEvent()
    {
        Console.WriteLine("The First Radio Message has been broadcast.");
    }
    
}
