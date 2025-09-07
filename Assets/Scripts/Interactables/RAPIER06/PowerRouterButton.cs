using UnityEngine;

public class PowerRouterButton : Interactable
{
    [SerializeField] private PowerCategory powerCat;
    [SerializeField] private bool increasePower = false;

    [SerializeField] private Material interactableMaterial;
    [SerializeField] private Material uninteractableMaterial;

    private const string INCREASE = "Increase ";
    private const string DECREASE = "Decrease ";
    private const string POWERLEVEL = " Power Level";

    private void Update()
    {
        isInteractable = !((increasePower && powerCat.powerLevel == 3)
            || (!increasePower && powerCat.powerLevel == 0)
            || PowerRouterController.Instance.onCooldown);

        GetComponent<Renderer>().material = isInteractable ? interactableMaterial : uninteractableMaterial;
    }

    protected override void Interact(Transform player)
    {
        if (increasePower) PowerRouterController.Instance.IncreasePowerLevel(powerCat);
        else PowerRouterController.Instance.LowerPowerLevel(powerCat);
    }

    public override string GetPrompt()
    {
        return (increasePower ? INCREASE : DECREASE) + powerCat.name + POWERLEVEL;
    }

}
