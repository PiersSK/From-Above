using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    public CharacterController controller;
    private Vector3 playerVelocity;

    private bool isGrounded;
    public bool movementOverridden = false;

    private bool sprinting = false;

    private bool crouching = false;
    private bool lerpCrouch = false;
    private float crouchTimer = 0f;

    public float gravity = -9.8f;
    public float currentSpeed = 3f;
    public float baseSpeed = 3f;
    public float sprintSpeed = 5f;
    public float jumpHeight = 1f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        currentSpeed = baseSpeed;
    }

    private void Update()
    {
        isGrounded = controller.isGrounded;
        if(lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;
            if (crouching)
                controller.height = Mathf.Lerp(controller.height, 1, p);
            else
                controller.height = Mathf.Lerp(controller.height, 2, p);

            if (p > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }
    }

    public void ProcessMove(Vector2 input)
    {
        if (movementOverridden) return;

        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        controller.Move(transform.TransformDirection(moveDirection) * currentSpeed * Time.deltaTime);

        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0) 
        {
            playerVelocity.y = -2f;
        }

        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGrounded && !movementOverridden)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }

    public void Crouch()
    {
        if(movementOverridden) return;
        crouching = !crouching;
        crouchTimer = 0f;
        lerpCrouch = true;
        if (crouching && sprinting) Sprint();
    }

    public void Sprint()
    {
        sprinting = !sprinting;
        if (sprinting) currentSpeed = sprintSpeed;
        else currentSpeed = baseSpeed;
    }

    public void ForcePlayerToPoint(Transform newPoint, bool useRotation = false)
    {
        controller.enabled = false;
        transform.position = newPoint.position;
        if (useRotation) transform.rotation = newPoint.rotation;
        controller.enabled = true;
    }

    public void ToggleMovementOverride()
    {
        movementOverridden = !movementOverridden;
    }
}
