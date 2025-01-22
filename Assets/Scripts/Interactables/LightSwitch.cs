using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : Interactable
{
    [SerializeField] private List<ShipLight> lights;

    protected override void Interact(Transform player)
    {
        foreach (var light in lights)
        {
            light.ToggleLight();
        }
    }
}
