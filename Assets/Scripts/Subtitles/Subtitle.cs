using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Subtitle", menuName = "Scriptable Objects/Subtitle")]
public class Subtitle : ScriptableObject
{
    public AudioClip clip;
    public string characterName;
    public UIColors.SubtitleColor trackColor;
    public List<string> dialogueLines = new();
    public List<int> dialogueTimestamps = new();
}
