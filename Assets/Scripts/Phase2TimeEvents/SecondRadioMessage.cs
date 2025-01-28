using System;

public class SecondRadioMessage : TimedEvent
{
    public override void TriggerEvent()
    {
        Console.WriteLine("The Second Radio Message has been broadcast.");
    }
    
}