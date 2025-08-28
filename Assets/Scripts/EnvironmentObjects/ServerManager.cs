using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ServerManager : MonoBehaviour
{
    [SerializeField] private List<ServerRack> servers;

    private void Start()
    {
        foreach(ServerRack r in servers)
        {
            r.SetLabel("Unit "+servers.IndexOf(r).ToString("00"));
        }
    }
}
