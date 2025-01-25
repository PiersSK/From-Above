using UnityEngine;

public class MusicPlayer : Interactable
{
    [SerializeField] private DataReader dataReader;
    [SerializeField] private AudioSource audioSource;

    private bool taskCompleted = false;
    [SerializeField] private DataDrive wellnessTape;
    [SerializeField] private Task wellnessTask;



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
    }
}
