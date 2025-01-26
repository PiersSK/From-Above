using UnityEngine;

public class MusicPlayer : Interactable
{
    [SerializeField] private DataReader dataReader;
    [SerializeField] private AudioSource audioSource;

    private bool taskCompleted = false;
    [SerializeField] private DataDrive wellnessTape;
    [SerializeField] private Task wellnessTask;

    [SerializeField] private Transform PD;
    [SerializeField] private Transform spindle;
    [SerializeField] private Transform horn;
    [SerializeField] private float spinTime = 2f;
    [SerializeField] private float pulseTime = 2f;
    private float playTime = 0f;

    private void Update()
    {
        if(audioSource.isPlaying)
        {
            playTime += Time.deltaTime;
            PD.localEulerAngles = new Vector3(-90f, 0f, 360f * (playTime / spinTime - Mathf.Floor(playTime / spinTime)));
            spindle.localEulerAngles = new Vector3(-90f, 0f, 360f * (playTime / spinTime - Mathf.Floor(playTime / spinTime)));

            float hornScale = 100f + 3f * Mathf.Sin((playTime / spinTime - Mathf.Floor(playTime / spinTime)) * pulseTime * Mathf.PI);
            horn.localScale = new Vector3(hornScale, hornScale, hornScale);
        }
    }

    protected override void Interact(Transform player)
    {
        DataDrive drive = dataReader.insertedDrive;
        if (drive != null)
        {
            //Load audio if first play
            if (audioSource.resource == null & drive.DiskAudioContent != null)
                audioSource.resource = drive.DiskAudioContent;

            if (audioSource.resource != null && !audioSource.isPlaying)
            {
                audioSource.Play();
                if (drive == wellnessTape && !taskCompleted) TaskManager.Instance.CompleteTask(wellnessTask);
            }
            else if (audioSource.isPlaying)
                audioSource.Pause();
           
        }
    }

    public void DiskRemoved()
    {
        audioSource.Stop();
        audioSource.resource = null;
        playTime = 0f;
        PD.localEulerAngles = new Vector3(-90f, 0f, 0f);
        spindle.localEulerAngles = new Vector3(-90f, 0f, 0f);
    }
}
