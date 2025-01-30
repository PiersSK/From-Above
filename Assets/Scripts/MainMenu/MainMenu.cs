using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class MainMenu : MonoBehaviour
{

    [SerializeField] float delayBeforeType = 0f;
    [SerializeField] float timeBtwChar = 0.1f;
    [SerializeField] string leadingChar = "|";
    [SerializeField] string writer;
    [SerializeField] TMP_Text _tmpProText;

    public void OnStartButton()
    {
        StartCoroutine("WriteText");
    }

    public IEnumerator WriteText()
    {
            _tmpProText.text = "";
            _tmpProText.text = leadingChar;

            yield return new WaitForSeconds(delayBeforeType);

            foreach (char c in writer)
            {
                if (_tmpProText.text.Length > 0)
                {
                    _tmpProText.text = _tmpProText.text.Substring(0, _tmpProText.text.Length - leadingChar.Length);
                }
                _tmpProText.text += c;
                _tmpProText.text += leadingChar;
                yield return new WaitForSeconds(timeBtwChar);
            }

            SceneManager.LoadScene(1);
        
    }
}
