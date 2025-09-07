using UnityEngine;
using UnityEngine.UI;

public class PowerIcon : MonoBehaviour
{
    [SerializeField] private Image allocateBlock;
    [SerializeField] private Image allocatedLockedBlock;
    [SerializeField] private Image unallocatedUnlockedBlock;

    [SerializeField] private bool allocated = false;
    [SerializeField] private bool locked = false;

    private void Start()
    {
        UpdateUIState();
    }

    private void UpdateUIState()
    {
        allocateBlock.gameObject.SetActive(false);
        allocatedLockedBlock.gameObject.SetActive(false);
        unallocatedUnlockedBlock.gameObject.SetActive(false);

        if (allocated && locked) allocatedLockedBlock.gameObject.SetActive(true);
        else if (allocated && !locked) allocateBlock.gameObject.SetActive(true);
        else if (!allocated && locked) unallocatedUnlockedBlock.gameObject.SetActive(true);
    }

    public void SetAllocatedState(bool state)
    {
        allocated = state;
        UpdateUIState();
    }

    public void SetLockedState(bool state)
    {
        locked = state;
        UpdateUIState();
    }

}
