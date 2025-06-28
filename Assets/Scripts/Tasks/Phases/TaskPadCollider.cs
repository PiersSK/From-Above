using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TaskPadCollider : MonoBehaviour
{
    [SerializeField] private Animator taskPadAnim;
    public List<GameObject> triggers = new();

    private void Update()
    {
        triggers.RemoveAll(x => !x.activeInHierarchy);
        if (triggers.Count == 0)
        {
            taskPadAnim.SetBool("IsObstructed", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        taskPadAnim.SetBool("IsObstructed", true);
        triggers.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        triggers.Remove(other.gameObject);
        if (triggers.Count == 0)
        {
            taskPadAnim.SetBool("IsObstructed", false);
        }
    }
}
