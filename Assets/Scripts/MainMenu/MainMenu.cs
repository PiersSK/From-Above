using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{

    [SerializeField] float delayBeforeType = 0f;
    [SerializeField] float timeBtwChar = 0.1f;
    [SerializeField] string leadingChar = "|";
    [SerializeField] string writer;
    [SerializeField] TMP_Text _tmpProText;
    [SerializeField] GameObject playButton;
    [SerializeField] GameObject commenceGameButton;
    [SerializeField] GameObject title;

    [SerializeField] private Animator anim;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip typesfx;

    private void OnEnable()
    {
        InputManager.InputTypeChanged += InputTypeChanged;
    }

    private void OnDisable()
    {
        InputManager.InputTypeChanged -= InputTypeChanged;
    }

    private void InputTypeChanged(InputManager.LastInputType type)
    {
        if (type == InputManager.LastInputType.KeyboardMouse)
        {
            GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(null);
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            if (playButton.activeSelf) playButton.GetComponent<Button>().Select();
            else if (commenceGameButton.activeSelf) commenceGameButton.GetComponent<Button>().Select();

            Cursor.lockState = CursorLockMode.Locked;
        }
    }

        public void OnStartButton()
    {
        anim.SetTrigger("Fade");
        title.SetActive(false);
        playButton.SetActive(false);
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

        commenceGameButton.SetActive(true);
        if(InputManager.Instance.GamepadIsCurrentInput()) commenceGameButton.GetComponent<Button>().Select();
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }
}
