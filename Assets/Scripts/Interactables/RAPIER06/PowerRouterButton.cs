using UnityEngine;

public class PowerRouterButton : Interactable
{
    [SerializeField] private PowerRouterController powerController;
    [SerializeField] private PowerCategory powerCat;
    [SerializeField] private bool increasePower = false;

    [SerializeField] private Material interactableMaterial;
    [SerializeField] private Material uninteractableMaterial;

    private const string INCREASE = "Increase ";
    private const string DECREASE = "Decrease ";
    private const string POWERLEVEL = " Power Level";

    private void Update()
    {
        isInteractable = !((increasePower && powerCat.powerLevel == 3) || (!increasePower && powerCat.powerLevel == 0));

        GetComponent<Renderer>().material = isInteractable ? interactableMaterial : uninteractableMaterial;
    }

    protected override void Interact(Transform player)
    {
        if (increasePower) powerController.IncreasePowerLevel(powerCat);
        else powerController.LowerPowerLevel(powerCat);
    }

    public override string GetPrompt()
    {
        return (increasePower ? INCREASE : DECREASE) + powerCat.name + POWERLEVEL;
    }

}
