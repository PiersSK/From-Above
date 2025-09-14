
using TMPro;
using UnityEngine;

public class KeycardTerminal : Computer
{
    [Header("Object References")]
    [SerializeField] private TextMeshProUGUI header;
    [SerializeField] private KeycardInput keycardInput;
    [SerializeField] private TextMeshProUGUI countdown;
    [SerializeField] private TextMeshProUGUI countdownHeader;

    override protected void Update()
    {
        screen.SetActive(TaskManager.Instance.currentPhase is WeaponTaskPhase);

        if (TimeController.Instance.isKeyCardTimerRunning)
            UpdateCountdownUI(TimeController.Instance.keycardTaskTimeLeft);
    }

    public void UpdateCountdownUI(float timeRemaining)
    {
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        int fraction = Mathf.FloorToInt(timeRemaining * 100f % 100);
        header.gameObject.SetActive(false);
        countdownHeader.gameObject.SetActive(true);
        countdown.gameObject.SetActive(true);
        countdown.text = $"{seconds:00}:{fraction:00}";
    }

    public void ShowFailureMessage()
    {
        countdown.gameObject.SetActive(false);
        countdownHeader.gameObject.SetActive(false);
        header.gameObject.SetActive(true);
        header.text = "UNLOCK FAILED.\n TRY AGAIN.";
    }

    public void ShowUnlockMessage()
    {
        countdown.gameObject.SetActive(false);
        countdownHeader.gameObject.SetActive(false);
        header.gameObject.SetActive(true);
        header.text = "WEAPON UNLOCKED";
    }
}