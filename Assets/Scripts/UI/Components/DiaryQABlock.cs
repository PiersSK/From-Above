using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiaryQABlock : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public List<Button> answerButtons = new List<Button>();

    public void LockInAnswer(Button btn)
    {
        btn.transform.localPosition = new Vector3(-32f, btn.transform.localPosition.y, 0f);
        btn.interactable = false;
        foreach(var button in answerButtons)
        {
            if (button != btn) button.gameObject.SetActive(false);
        }
    }
}
