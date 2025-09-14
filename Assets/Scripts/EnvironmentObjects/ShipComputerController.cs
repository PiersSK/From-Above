using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShipComputerController : MonoBehaviour
{
    public static ShipComputerController Instance;
    [SerializeField] private List<GameObject> otherScreens = new();
    [SerializeField] private List<Interactable> otherInteractables = new();
    private List<Computer> computers = new();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        computers = FindObjectsByType<Computer>(FindObjectsSortMode.None).ToList();
    }

    public void SwitchOffAllComputers()
    {
        foreach (Computer c in computers) c.SwitchOffComputer();
        foreach (GameObject o in otherScreens) o.SetActive(false);
        foreach (Interactable i in otherInteractables) i.isInteractable = false;
    }

    public void SwitchOnAllComputers()
    {
        foreach (Computer c in computers) c.SwitchOnComputer();
        foreach (GameObject o in otherScreens) o.SetActive(true);
        foreach (Interactable i in otherInteractables) i.isInteractable = true;
    }
}
