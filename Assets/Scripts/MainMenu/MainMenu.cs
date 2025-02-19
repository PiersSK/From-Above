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
    [SerializeField] GameObject btn;
    [SerializeField] GameObject startBtn;
    [SerializeField] GameObject title;

    [SerializeField] private Animator anim;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip typesfx;

    public void OnStartButton()
    {
        anim.SetTrigger("Fade");
        title.SetActive(false);
        btn.SetActive(false);
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
                source.PlayOneShot(typesfx);
                yield return new WaitForSeconds(timeBtwChar * (c == '\\' ? 5 : 1));
            }

        startBtn.SetActive(true);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }
}
