using UnityEngine;

[CreateAssetMenu(fileName = "DataDrive", menuName = "Scriptable Objects/DataDrive")]
public class DataDrive : ScriptableObject
{
    public string DiskName;
    public string DiskTextContent;
}
