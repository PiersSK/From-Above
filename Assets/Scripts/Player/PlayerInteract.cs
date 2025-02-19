using TMPro;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private float distance = 3f;
    [SerializeField] private LayerMask mask;
    [SerializeField] private TextMeshProUGUI promptText;

    private InputManager inputManager;

    private void Start()
    {
        cam = GetComponent<PlayerLook>().cam;
        inputManager = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        promptText.text = string.Empty;

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            if(hitInfo.collider.GetComponent<Interactable>() != null)
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();

                if (interactable.CanInteract())
                {
                    promptText.text = "[E] " + interactable.GetPrompt();

                    if (inputManager.playerActions.Interact.triggered)
                    {
                        interactable.BaseInteract(transform);
                    }
                }
            }
        }

    }
}
