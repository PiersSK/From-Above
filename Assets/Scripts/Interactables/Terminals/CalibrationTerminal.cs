using System.Collections;
using UnityEngine;

public class CalibrationTerminal : Computer
{
    [SerializeField] private CalibrationUI cal;
    [SerializeField] private AudioClip scrubbingSound;

    [SerializeField] private float scrubbingSoundFrequency = 5f;

    private bool soundCooldown = false;
    private bool locationLocked = false;
    private bool lockedOut = false;

    private const string SCROLLMAP = "To Scroll Map";
    private const string LOCKTARGET = "Confirm Authorised Target Lost";

    protected override void Update()
    {
        if(playerAtComputer)
        {
            Vector2 moveInput = InputManager.Instance.playerActions.Move.ReadValue<Vector2>();

            if (!locationLocked)
            {
                cal.MoveMap(-moveInput);
                cal.CheckCrosshairSnapping(moveInput);
                if(moveInput != Vector2.zero)
                {
                    if (!soundCooldown)
                    {
                        SoundManager.Instance.PlaySFXOneShot(scrubbingSound, 0, 0.05f);
                        soundCooldown = true;
                        StartCoroutine(StartCooldown());
                    }
                }

                if (cal.CorrectTargetFound()) UIManager.Instance.ShowButtonPrompt(UIManager.ButtonPromptType.Confirm, LOCKTARGET);
                else UIManager.Instance.HideButtonPrompt(UIManager.ButtonPromptType.Confirm);

                if(InputManager.Instance.playerActions.Interact.triggered && cal.CorrectTargetFound())
                {
                    locationLocked = true;
                    UIManager.Instance.HideButtonPrompt(UIManager.ButtonPromptType.Move);
                    UIManager.Instance.HideButtonPrompt(UIManager.ButtonPromptType.Confirm);
                    UIManager.Instance.HideButtonPrompt(UIManager.ButtonPromptType.BackOut);
                }

                if (!isInteractable && InputManager.Instance.playerActions.Escape.triggered)
                {
                    ReleasePlayer();
                }
            }
            else
            {
                float completionVal = cal.UpdateCalibrationMinigame(moveInput);
                if(completionVal == 0)
                {
                    Invoke("UnlockTarget", cal.cooldownLength);
                    lockedOut = true;
                    ReleasePlayer();
                } else if (completionVal == 1)
                {
                    lockedOut = true;
                    ReleasePlayer();
                }
            }

        }
    }

    private void UnlockTarget()
    {
        locationLocked = false;
        lockedOut = false;
        isInteractable = true;
    }

    private IEnumerator StartCooldown()
    {
        float end = 1 / scrubbingSoundFrequency;
        float elapsed = 0f;

        while(elapsed < end)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        soundCooldown = false;
    }

    public void InitialEnable()
    {
        isInteractable = true;
    }

    protected override void Interact(Transform player)
    {
        base.Interact(player);
        UIManager.Instance.ShowButtonPrompt(UIManager.ButtonPromptType.Move, SCROLLMAP);
    }

    protected override void ReleasePlayer()
    {
        base.ReleasePlayer();
        if(lockedOut) isInteractable = false;
        UIManager.Instance.HideButtonPrompt(UIManager.ButtonPromptType.Move);
        UIManager.Instance.HideButtonPrompt(UIManager.ButtonPromptType.Confirm);
    }
}
