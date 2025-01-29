using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SubtitleManager : MonoBehaviour
{
    public List<Subtitle> subtitleTracks;
    public TextMeshProUGUI subtitleText;
    public Transform subtitleHolder;

    private void Update()
    {
        int lineIndex = 0;

        List<AudioClip> subtitledClips = subtitleTracks.Select(x => x.clip).ToList();
        List<AudioSource> subtitledAudios = new List<AudioSource>(FindObjectsByType<AudioSource>(FindObjectsSortMode.None)).Where(x => x.isPlaying && subtitledClips.Contains(x.clip)).ToList();

        Debug.Log("SubtitledAudios Count = " + subtitledAudios.Count);
        foreach (Transform t in subtitleHolder) Destroy(t.gameObject);

        foreach(AudioSource a in subtitledAudios)
        {
            Subtitle s = subtitleTracks.Where(x => x.clip == a.clip).DefaultIfEmpty(null).Min();
            if (s != null)
            {
                int relevantTimestamp = s.dialogueTimestamps.Where(x => x < a.time).DefaultIfEmpty(-1).Max();
                lineIndex = relevantTimestamp > 0 ? s.dialogueTimestamps.IndexOf(relevantTimestamp) : 0;
                TextMeshProUGUI line = Instantiate(Resources.Load<TextMeshProUGUI>("Subtitle"), subtitleHolder);
                line.text = "[" + s.characterName + "] - " + s.dialogueLines[lineIndex];
                line.color = s.trackColor != null ? s.trackColor : Color.white;
            }
        }
    }
}
