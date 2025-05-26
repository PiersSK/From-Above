
using TMPro;
using UnityEngine;

public class KeycardTerminal : Computer
{
    [Header("Object References")]
    [SerializeField] private TextMeshProUGUI Header;
    [SerializeField] private KeycardInput keycardInput;
    [SerializeField] private TextMeshProUGUI Countdown;
    [SerializeField] private TextMeshProUGUI CountdownHeader;

    private void Start()
    {

    }

    override protected void Update()
    {
        if (TimeController.Instance.isKeyCardTimerRunning)
            UpdateCountdownUI(TimeController.Instance.keycardTaskTimeLeft);
    }

    public void UpdateCountdownUI(float timeRemaining)
    {
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        int fraction = Mathf.FloorToInt(timeRemaining * 100f % 100);
        Header.gameObject.SetActive(false);
        CountdownHeader.gameObject.SetActive(true);
        Countdown.gameObject.SetActive(true);
        Countdown.text = $"{seconds:00}:{fraction:00}";
    }

    public void ShowFailureMessage()
    {
        Countdown.gameObject.SetActive(false);
        CountdownHeader.gameObject.SetActive(false);
        Header.gameObject.SetActive(true);
        Header.text = "UNLOCK FAILED.\n TRY AGAIN.";
    }

    public void ShowUnlockMessage()
    {
        Countdown.gameObject.SetActive(false);
        CountdownHeader.gameObject.SetActive(false);
        Header.gameObject.SetActive(true);
        Header.text = "WEAPON UNLOCKED";
    }
}