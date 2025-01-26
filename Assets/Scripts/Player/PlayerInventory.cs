using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }
    public List<DataDrive> dataDrivesHeld;

    private void Awake()
    {
        Instance = this;
    }
}
