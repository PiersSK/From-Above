using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SubtitleManager : MonoBehaviour
{
    public List<Subtitle> subtitleTracks;
    public TextMeshProUGUI subtitleText;

    private void Update()
    {
        int lineIndex = 0;
        bool subtitlePlaying = false;
        foreach(AudioSource a in FindObjectsByType<AudioSource>(FindObjectsSortMode.None))
        {
            if(a.isPlaying && a.clip != null)
            {
                Subtitle s = subtitleTracks.Where(x => x.clip == a.clip).DefaultIfEmpty(null).Min();
                if(s != null)
                { 
                    int relevantTimestamp = s.dialogueTimestamps.Where(x => x < a.time).DefaultIfEmpty(-1).Max();
                    lineIndex = relevantTimestamp > 0 ? s.dialogueTimestamps.IndexOf(relevantTimestamp) : 0;
                    subtitleText.text = s.dialogueLines[lineIndex];
                    subtitlePlaying = true;
                }
            }
            if (subtitlePlaying) break;
        }

        if (!subtitlePlaying) subtitleText.text = string.Empty;

    }
}
