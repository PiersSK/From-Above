using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }
    public List<DataDrive> dataDrivesHeld;
    [SerializeField] private DataDrive stripedPD;
    [SerializeField] private GameObject stripedPDObj;
    private int drivesLastHeld = 0;

    [SerializeField] private Transform PDHolder;
    [SerializeField] private GameObject keycard1;
    [SerializeField] private GameObject keycard2;

    public bool hasKeycard1 = false;
    public bool hasKeycard2 = false;

    private void Awake()
    {
        Instance = this;
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
                    int i = dataDrivesHeld.IndexOf(drive);
                    Transform t = Instantiate(Resources.Load<Transform>("PD"), PDHolder);
                    t.localPosition = new Vector3(i * 0.01f, i * 0.1f, 0f);
                }
            }
        }

        keycard1.SetActive(hasKeycard1);
        keycard2.SetActive(hasKeycard2);
    }
}
