using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SubtitleManager : MonoBehaviour
{
    public List<Subtitle> subtitleTracks;
    public Transform subtitleHolder;
    [SerializeField] private Transform player;
    [SerializeField] private int subtitle3DRange;

    private void Start()
    {
        foreach(Subtitle subtitleTrack in subtitleTracks)
        {
            Debug.Log("Name: " + subtitleTrack.name + " Clip: " + subtitleTrack.clip);
        }
    }

    private void Update()
    {
        int lineIndex = 0;

        List<AudioClip> subtitledClips = subtitleTracks.Select(x => x.clip).ToList();
        List<AudioSource> subtitledAudios = new List<AudioSource>(FindObjectsByType<AudioSource>(FindObjectsSortMode.None)).Where(x => x.isPlaying && subtitledClips.Contains(x.clip)).ToList();

        foreach (Transform t in subtitleHolder) Destroy(t.gameObject);

        foreach(AudioSource a in subtitledAudios)
        {
            Debug.Log(a.clip);
            Subtitle s = subtitleTracks.Where(x => x.clip == a.clip).DefaultIfEmpty(null).Min();
            if (a.spatialBlend > 0.5 && Vector3.Distance(player.position, a.transform.position) > subtitle3DRange) continue;

            if (s != null)
            {
                int relevantTimestamp = s.dialogueTimestamps.Where(x => x < a.time).DefaultIfEmpty(-1).Max();
                lineIndex = relevantTimestamp > 0 ? s.dialogueTimestamps.IndexOf(relevantTimestamp) : 0;
                TextMeshProUGUI line = Instantiate(Resources.Load<TextMeshProUGUI>("Subtitle"), subtitleHolder);
                string nametag = s.dialogueLines[lineIndex] != "" && s.dialogueLines[lineIndex] != " " && s.dialogueLines[lineIndex] != string.Empty ? "[" + s.characterName + "] - " : string.Empty;
                line.text = nametag + s.dialogueLines[lineIndex];
                line.color = s.trackColor != null ? s.trackColor : Color.white;
            }
        }
    }
}
