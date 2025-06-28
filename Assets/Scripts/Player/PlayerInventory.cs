using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }
    public List<IServerDataObject> dataDrivesHeld;
    public List<IServerDataObject> exesHeld;
    [SerializeField] private DataDrive stripedPD;
    [SerializeField] private GameObject stripedPDObj;
    private int drivesLastHeld = 0;
    private int exesLastHeld = 0;

    [SerializeField] private Transform PDHolder;
    [SerializeField] private Transform FCHolder;
    [SerializeField] private GameObject keycard1;
    [SerializeField] private GameObject keycard2;
    [SerializeField] private GameObject serverPassword;

    public bool hasKeycard1 = false;
    public bool hasKeycard2 = false;
    public bool hasServerPassword = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (dataDrivesHeld.Count != drivesLastHeld)
        {
            drivesLastHeld = dataDrivesHeld.Count;

            stripedPDObj.SetActive(dataDrivesHeld.Contains(stripedPD));

            foreach (Transform t in PDHolder) Destroy(t.gameObject);
            foreach (DataDrive drive in dataDrivesHeld)
            {
                if (drive != stripedPD)
                {
                    int i = dataDrivesHeld.IndexOf(drive) % 5;
                    int j = Mathf.FloorToInt(dataDrivesHeld.IndexOf(drive) / 5);
                    Transform t = Instantiate(Resources.Load<Transform>("PD"), PDHolder);
                    t.localPosition = new Vector3(i * 0.01f + j * 0.05f, i * 0.1f, j * -0.1f);
                }
            }
        }

        if (exesHeld.Count != exesLastHeld)
        {
            exesLastHeld = exesHeld.Count;

            foreach (Transform t in FCHolder) Destroy(t.gameObject);
            foreach (ServerExe exe in exesHeld)
            {
                int i = exesHeld.IndexOf(exe);
                Transform t = Instantiate(Resources.Load<Transform>("FC"), FCHolder);
                t.localPosition = new Vector3(i * 0.01f, i * 0.05f, 1.234f + i * 0.01f);
            }
        }

        keycard1.SetActive(hasKeycard1);
        keycard2.SetActive(hasKeycard2);
        serverPassword.SetActive(hasServerPassword);
    }

    public List<IServerDataObject> AudioDrivesHeld()
    {
        List<IServerDataObject> audioDrives = new();
        foreach(IServerDataObject dataDrive in dataDrivesHeld)
        {
            DataDrive d = (DataDrive)dataDrive;
            foreach(DiscSlotContent dsc in ServerHubUI.Instance.pdStorage.GetPDSlots(d))
            {
                if (dsc is AudioContent a) audioDrives.Add(d);
            }
        }

        return audioDrives;
    }
}
