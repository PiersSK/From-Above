using TMPro;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private float distance = 3f;
    [SerializeField] private LayerMask mask;
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private GameObject buttonPromptObject;

    private void Start()
    {
        cam = GetComponent<PlayerLook>().cam;
    }

    // Update is called once per frame
    void Update()
    {
        promptText.text = string.Empty;
        buttonPromptObject.SetActive(false);

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
                    promptText.color = UIColors.white;
                    promptText.text = interactable.GetPrompt();
                    buttonPromptObject.SetActive(true);

                    bool triggerCheck = interactable is HoldInteractable ?
                        InputManager.Instance.playerActions.Interact.IsPressed() :
                        InputManager.Instance.playerActions.Interact.triggered;

                    if (triggerCheck)
                    {
                        interactable.BaseInteract(transform);
                    }
                    else if(interactable is HoldInteractable)
                    {
                        (interactable as HoldInteractable).baseCancelInteract(transform);
                    }

                } else
                {
                    promptText.color = UIColors.terminalRed;
                    promptText.text = interactable.GetRequirementMessage();
                }
            }
        }

    }
}
