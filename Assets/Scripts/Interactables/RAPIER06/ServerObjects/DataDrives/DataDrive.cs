using UnityEngine;

[CreateAssetMenu(fileName = "DataDrive", menuName = "Scriptable Objects/DataDrive")]
public class DataDrive : IServerDataObject
{
    [TextArea(15,20)]
    public string textContent;
    public AudioClip audioContent;
}
