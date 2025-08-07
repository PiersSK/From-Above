using UnityEngine;

public class CalibrationTerminal : Computer
{
    [SerializeField] private CalibrationUI cal;

    protected override void Update()
    {
        base.Update();
        if(playerAtComputer)
        {
            Vector2 moveInput = InputManager.Instance.playerActions.Move.ReadValue<Vector2>();
            cal.MoveMap(-moveInput);
        }
    }
}
