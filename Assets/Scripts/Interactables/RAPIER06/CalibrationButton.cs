using UnityEngine;

public class CalibrationButton : HoldInteractable
{
    public enum ButtonDirection
    {
        Left,
        Right, 
        Up,
        Down
    }
    [SerializeField] private ButtonDirection direction;
    [SerializeField] private CalibrationUI ui;

    protected override void Interact(Transform player)
    {
        ui.MoveMap(direction);

        base.Interact(player);
    }
}
