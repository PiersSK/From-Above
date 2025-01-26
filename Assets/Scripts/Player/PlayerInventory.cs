using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }
    public List<DataDrive> dataDrivesHeld;
    private int drivesLastHeld = 0;

    [SerializeField] private Transform PDHolder;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (dataDrivesHeld.Count != drivesLastHeld)
        {
            drivesLastHeld = dataDrivesHeld.Count;
            foreach (Transform t in PDHolder) Destroy(t.gameObject);

            foreach (DataDrive drive in dataDrivesHeld)
            {
                int i = dataDrivesHeld.IndexOf(drive);
                Transform t = Instantiate(Resources.Load<Transform>("PD"), PDHolder);
                t.localPosition = new Vector3(i * 0.01f, i * 0.1f, 0f);
            }
        }
        { }
    }
}
