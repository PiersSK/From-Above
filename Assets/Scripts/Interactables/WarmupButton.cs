using UnityEngine;

public class WarmupButton : Interactable
{
    [SerializeField] private Transform symbol;
    [SerializeField] private Material glowMaterial;

    private void OnEnable()
    {
        TaskManager.PhaseChanged += OnPhaseChange;
    }

    private void OnPhaseChange()
    {
        if (TaskManager.Instance.currentPhase is WeaponTaskPhase)
        {
            foreach(Transform t in symbol)
            {
                t.GetComponent<Renderer>().material = glowMaterial;
            }
            isInteractable = true;
            TaskManager.PhaseChanged -= OnPhaseChange;
        }
    }

    protected override void Interact(Transform player)
    {
        base.Interact(player);
        isInteractable = false;
    }
}
