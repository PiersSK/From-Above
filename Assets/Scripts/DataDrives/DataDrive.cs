using UnityEngine;

[CreateAssetMenu(fileName = "DataDrive", menuName = "Scriptable Objects/DataDrive")]
public class DataDrive : ScriptableObject
{
    public string DiskName;
    [TextArea(15,20)]
    public string DiskTextContent;
    public AudioClip DiskAudioContent;
}
