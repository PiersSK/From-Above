using UnityEngine;

public class TaskManager : MonoBehaviour
{
    private bool taskPadObtained = false;
    private bool taskPadVisible = false;

    private Animator taskPadAnim;
    [SerializeField] private GameObject taskPadObj;

    private void Start()
    {
        taskPadAnim = taskPadObj.GetComponent<Animator>();
    }

    public void ObtainTaskpad()
    {
        taskPadObtained = true;
        taskPadObj.SetActive(true);
        ToggleTaskPad();
    }
    public void ToggleTaskPad()
    {
        taskPadVisible = !taskPadVisible;
        UIManager.Instance.ToggleMenuPromptStatus();
        taskPadAnim.SetBool("IsUp", taskPadVisible);
    }
}
