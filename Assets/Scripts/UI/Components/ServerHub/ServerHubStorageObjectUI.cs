using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ServerHubStorageObjectUI : MonoBehaviour
{
    public void SetStorageObject(IServerDataObject dataObject)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = dataObject.name;
    }
}
