using System.Collections;
using UnityEngine;

public class CalibrationTerminal : Computer
{
    [SerializeField] private CalibrationUI cal;
    [SerializeField] private AudioClip scrubbingSound;

    [SerializeField] private float scrubbingSoundFrequency = 5f;
    private bool soundCooldown = false;

    private const string SCROLLMAP = "To Scroll Map";

    protected override void Update()
    {
        base.Update();
        if(playerAtComputer)
        {
            Vector2 moveInput = InputManager.Instance.playerActions.Move.ReadValue<Vector2>();
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
        }
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
