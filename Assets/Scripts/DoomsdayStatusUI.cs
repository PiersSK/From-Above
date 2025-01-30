using TMPro;
using UnityEngine;

public class DoomsdayStatusUI : MonoBehaviour
{
    public static DoomsdayStatusUI Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI authStatus;
    [SerializeField] private GameObject stepsUI;
    [SerializeField] private GameObject overrideUI;
    [SerializeField] private TextMeshProUGUI initiationStatus;
    [SerializeField] private TextMeshProUGUI targetingStatus;
    [SerializeField] private TextMeshProUGUI unlockStatus;
    [SerializeField] private TextMeshProUGUI deployStatus;
    [SerializeField] private TextMeshProUGUI safetyStatus;
    [SerializeField] private GameObject fireMessage;

    public bool warmedUp = false;
    public int calibrated = 0;
    public int keycardsInserted = 0;
    public int weaponLeversPulled = 0;
    public bool safetyOff = false;

    private bool fireIsReady = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        authStatus.text = TaskManager.Instance.isPhaseTwo ? "WEAPON USE AUTHORISED" : "USE CURRENTLY UNAUTHORISED";
        stepsUI.SetActive(TaskManager.Instance.isPhaseTwo && !fireIsReady);

        initiationStatus.text = warmedUp ? "WEAPON CORE WARMED UP" : "NOT STARTED";
        targetingStatus.text = calibrated.ToString() + "/2 CALIBRATED";
        unlockStatus.text = keycardsInserted.ToString() + "/2 KEYCARDS";
        deployStatus.text = weaponLeversPulled.ToString() + "/3 LEVERS";
        safetyStatus.text = safetyOff.ToString().ToUpper();

        fireIsReady = warmedUp && safetyOff && calibrated == 2 && weaponLeversPulled == 3 && keycardsInserted == 2;
        fireMessage.SetActive(fireIsReady);

        overrideUI.SetActive(TaskManager.Instance.pacifistEndingReached);
    }



}
