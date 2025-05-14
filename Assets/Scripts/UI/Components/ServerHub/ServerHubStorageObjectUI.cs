using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ServerHubStorageObjectUI : MonoBehaviour
{
    public bool accessible = true;

    public void SetStorageObject(IServerDataObject dataObject, List<IServerDataObject> filterList = null)
    {
        string displayName = dataObject.objectName;
        if(filterList != null)
        {
            if(!filterList.Contains(dataObject))
            {
                accessible = false;
                displayName = "???";
            }
        } else
        {
            accessible = true;
        }


        GetComponentInChildren<TextMeshProUGUI>().text = displayName;
    }
}
