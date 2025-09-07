using System.Collections;
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

    public Image GetRelevantBlock()
    {
        if (allocated && locked) return allocatedLockedBlock;
        else if (allocated && !locked) return allocateBlock;
        else if (!allocated && locked) return unallocatedUnlockedBlock;

        return allocateBlock;
    }

    private void UpdateUIState()
    {
        allocateBlock.gameObject.SetActive(false);
        allocatedLockedBlock.gameObject.SetActive(false);
        unallocatedUnlockedBlock.gameObject.SetActive(false);

        GetRelevantBlock().gameObject.SetActive(true);
    }

    public void SetAllocatedState(bool state)
    {
        allocated = state;
        StartCoroutine(ChangeBatteryPower(state));
        UpdateUIState();
    }

    public void SetLockedState(bool state)
    {
        locked = state;
        UpdateUIState();
    }

    private IEnumerator ChangeBatteryPower(bool increase = true)
    {
        float timer = 0f;
        float timeToChange = PowerRouterController.Instance.powerChangeTransitionTime;
        PowerRouterController.Instance.onCooldown = true;

        while (timer < timeToChange)
        {
            float p = timer / timeToChange;
            timer += Time.deltaTime;

            if (!increase) p = 1 - p;

            GetRelevantBlock().fillAmount = p;

            yield return null;
        }

        GetRelevantBlock().fillAmount = increase ? 1 : 0;
        PowerRouterController.Instance.onCooldown = false;

    }
}
