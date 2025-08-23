
using UnityEngine;

public class ServerHubTerminal : Computer
{

    protected override void Update()
    {
        if (!isInteractable && InputManager.Instance.playerActions.Escape.triggered)
        {
            if (InputManager.Instance.GamepadIsCurrentInput())
            {
                bool contextualReturnUsed = ServerHubUI.Instance.GamepadReturnPressed();
                if (!contextualReturnUsed && !ServerHubUI.Instance.longerAnimationPlaying) ReleasePlayer();
            }
            else
            {
                ReleasePlayer();
            }
        }
    }
    protected override void Interact(Transform player)
    {
        base.Interact(player);
        ServerHubUI.Instance.SelectRelevantStartButton();
    }

    protected override void SwitchToGamepad()
    {
        Cursor.lockState = CursorLockMode.Locked;
        ServerHubUI.Instance.SelectRelevantStartButton();
        UIManager.Instance.ShowButtonPrompt(UIManager.ButtonPromptType.BackOut, ServerHubUI.Instance.GetGamepadBackoutPrompt());
    }
}
