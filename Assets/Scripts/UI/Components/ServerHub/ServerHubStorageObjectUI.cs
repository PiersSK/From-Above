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
        GetComponent<Image>().color = UIColors.terminalGreen;
        if(filterList != null)
        {
            if(!filterList.Contains(dataObject))
            {
                accessible = false;
                displayName = "???";
                GetComponent<Image>().color = UIColors.darkGrey;
            }
        } else
        {
            accessible = true;
        }


        GetComponentInChildren<TextMeshProUGUI>().text = displayName;
    }
}
