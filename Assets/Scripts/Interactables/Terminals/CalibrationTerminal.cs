using UnityEngine;

public class CalibrationTerminal : Computer
{
    [SerializeField] private CalibrationUI cal;

    private const string SCROLLMAP = "To Scroll Map";

    protected override void Update()
    {
        base.Update();
        if(playerAtComputer)
        {
            Vector2 moveInput = InputManager.Instance.playerActions.Move.ReadValue<Vector2>();
            cal.MoveMap(-moveInput);
        }
    }

    protected override void Interact(Transform player)
    {
        base.Interact(player);
        UIManager.Instance.ShowButtonPrompt(UIManager.ButtonPromptType.Move, SCROLLMAP);
    }

    protected override void ReleasePlayer()
    {
        base.ReleasePlayer();
        UIManager.Instance.HideButtonPrompt(UIManager.ButtonPromptType.Move);
    }
}
