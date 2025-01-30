using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class IntroText : MonoBehaviour
{

    Text _text;
    TMP_Text _tmpProText;
    string writer;
    bool textScrollFinished = false;

    [SerializeField] float delayBeforeType = 0f;
    [SerializeField] float timeBtwChar = 0.1f;
    [SerializeField] string leadingChar = "";

    void Start()
    {
        _text = GetComponent<Text>();
        _tmpProText = GetComponent<TMP_Text>();

        if( _text != null)
        {
            writer = _text.text;
            _text.text = "";

            StartCoroutine("TypeWriterText");
        }

        if( _tmpProText != null)
        {
            writer = _tmpProText.text;
            _tmpProText.text = "";

            StartCoroutine("TypeWriterTMP");
        }
    }

    IEnumerator TypeWriterTMP()
    {
        _tmpProText.text = leadingChar;

        yield return new WaitForSeconds(delayBeforeType);

        foreach(char c in writer)
        {
            if(_tmpProText.text.Length > 0)
            {
                _tmpProText.text = _tmpProText.text.Substring(0, _tmpProText.text.Length - leadingChar.Length);
            }
            _tmpProText.text += c;
            _tmpProText.text += leadingChar;
            yield return new WaitForSeconds(timeBtwChar);
        }

        if ( leadingChar != "")
        {
            _tmpProText.text = _tmpProText.text.Substring(0, _tmpProText.text.Length - leadingChar.Length);
            textScrollFinished = true;
        }
    }
}
