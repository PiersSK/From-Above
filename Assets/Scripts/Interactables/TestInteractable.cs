using UnityEngine;

public class TestInteractable : Interactable
{
    [SerializeField] private Material redMat;
    [SerializeField] private Material blueMat;

    private bool isRed = true;

    protected override void Interact(Transform player)
    {
        Debug.Log("Interacted with " + gameObject.name);
        isRed = !isRed;

        if (isRed) gameObject.GetComponent<Renderer>().material = redMat;
        else gameObject.GetComponent<Renderer>().material = blueMat;
    }
}
