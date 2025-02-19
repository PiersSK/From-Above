using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Ladder : Interactable
{
    [SerializeField] private Transform topLockPoint;
    [SerializeField] private Transform bottomLockPoint;
    [SerializeField] private Transform topDisembarkPoint;

    private float sfxTimer = 0f;
    [SerializeField] float sfxSpacing = 0.6f;
    [SerializeField] List<AudioClip> sounds;
    private bool playerOnLadder = false;
    private Transform player; 

    private void Update()
    {
        if (playerOnLadder)
        {
            PlayerMotor motor = player.GetComponent<PlayerMotor>();
            InputManager playerInput = player.GetComponent<InputManager>();

            float input = playerInput.playerActions.Move.ReadValue<Vector2>().y;
            Vector3 moveDirection = new Vector3(0f, input, 0f);
            motor.controller.Move(transform.TransformDirection(moveDirection) * 2f * Time.deltaTime);

            bool atTopOfLadder = player.position.y > (topLockPoint.position.y + 1f);

            if (motor.controller.isGrounded || atTopOfLadder)
            {
                ToggleLadderState();
            }

            if (input != 0)
            {
                sfxTimer += Time.deltaTime;
                if (sfxTimer >= 0.6f)
                {
                    SoundManager.Instance.PlaySFXOneShot(sounds[Random.Range(0, sounds.Count)]);
                    sfxTimer = 0f;
                }
            }

        }
    }

    private void ToggleLadderState(bool SnapToPoint = true) {
        playerOnLadder = !playerOnLadder;
        if(playerOnLadder) SoundManager.Instance.PlaySFXOneShot(sounds[Random.Range(0, sounds.Count)]);
        promptMessage = playerOnLadder ? "Release Ladder" : "Use Ladder";
        PlayerMotor motor = player.GetComponent<PlayerMotor>();

        if (SnapToPoint)
        {
            float topDistance = Vector3.Distance(player.position, topLockPoint.position);
            float bottomDistance = Vector3.Distance(player.position, bottomLockPoint.position);
            Transform lockPoint = bottomLockPoint;

            if (playerOnLadder)
                lockPoint = topDistance < bottomDistance ? topLockPoint : bottomLockPoint;
            else
                lockPoint = topDistance < bottomDistance ? topDisembarkPoint : bottomLockPoint;

            motor.ForcePlayerToPoint(lockPoint, true);
        }

        sfxTimer = 0f;
        motor.ToggleMovementOverride();
    }


    protected override void Interact(Transform p)
    {
        player = p;
        ToggleLadderState(!playerOnLadder);
    }
}
