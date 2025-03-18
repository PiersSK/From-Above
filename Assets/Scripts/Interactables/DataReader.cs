using NUnit;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataReader : Interactable
{
    public DataDrive insertedDrive = null;
    [SerializeField] private TextMeshProUGUI insertedDriveName;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject visiblePD;


    [SerializeField] private GameObject defaultScreen;
    [SerializeField] private GameObject outputScreen;
    [SerializeField] private TextMeshProUGUI textOutput;
    [SerializeField] private MusicPlayer audioOutput;

    [SerializeField] private AudioClip ejectSfx;
    [SerializeField] private AudioClip insertSfx;
    [SerializeField] private AudioClip audioDiscSfx;

    [SerializeField] private GameObject PDUI;
    [SerializeField] private Transform PDUIButtonContainer;
    [SerializeField] private Button PDCancelBtn;

    private Transform player;
    private const string NODISK = "NONE INSERTED";
    private List<Button> pdButtons = new();

    public override bool CanInteract()
    {
        return PlayerInventory.Instance.dataDrivesHeld.Count > 0 || insertedDrive != null;
    }

    public override string GetPrompt()
    {
        string addPrefix = audioOutput != null ? "Place" : "Insert";
        return insertedDrive != null ? "Take " + insertedDrive.DiskName + " PD" : addPrefix + "PD";
    }

    protected override void Interact(Transform p)
    {
        player = p;
        PlayerInventory inv = PlayerInventory.Instance;

        if (insertedDrive != null)
        {
            UnloadDrive();
        }
        else
        {
            if(!InputManager.Instance.GamepadIsCurrentInput()) Cursor.lockState = CursorLockMode.None;
            InputManager.InputTypeChanged += InputChangedWhilstUIOpen;
            player.GetComponent<PlayerMotor>().ToggleMovementOverride();
            player.GetComponent<PlayerLook>().ToggleLookLock();
            UIManager.Instance.ToggleCrosshairVisibility();

            foreach (Transform t in PDUIButtonContainer) Destroy(t.gameObject);

            pdButtons.Clear();
            foreach (DataDrive d in inv.dataDrivesHeld)
            {
                Button b = Instantiate(Resources.Load<Button>("PDButton"), PDUIButtonContainer);
                b.GetComponent<PDButton>().SetDrive(d, this);

                pdButtons.Add(b);
                if (inv.dataDrivesHeld.IndexOf(d) == 0 && InputManager.Instance.GamepadIsCurrentInput()) b.Select();
            }

            foreach(Button b in pdButtons)
            {
                int index = pdButtons.IndexOf(b);

                Selectable up = index > 1 ? pdButtons[index - 2] : null;
                Selectable down = index < pdButtons.Count - (2 - index % 2) ? pdButtons[index + 2 >= pdButtons.Count ? pdButtons.Count - 1 : index + 2] : PDCancelBtn;
                Selectable left = index % 2 == 1 ? pdButtons[index - 1] : null;
                Selectable right = index % 2 == 0 && index < pdButtons.Count - 1 ? pdButtons[index + 1] : null;

                b.navigation = UIManager.Instance.CreateNewNavigation(up, down, left, right);
            }

            PDCancelBtn.navigation = UIManager.Instance.CreateNewNavigation(pdButtons.Count > 0 ? pdButtons[pdButtons.Count - 1] : null, null, null, null);

            PDUI.SetActive(true);
        }
    }

    private void InputChangedWhilstUIOpen(InputManager.LastInputType newType)
    {
        if (newType == InputManager.LastInputType.KeyboardMouse)
        {
            Cursor.lockState = CursorLockMode.None;
            UIManager.Instance.ClearSelectedUIObject();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            if (pdButtons.Count > 0)
                pdButtons[0].Select();
            else
                PDCancelBtn.Select();
        }
    }

    public void UnlockPlayer()
    {
        Transform player = PlayerInventory.Instance.transform;

        PDUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        player.GetComponent<PlayerMotor>().ToggleMovementOverride();
        player.GetComponent<PlayerLook>().ToggleLookLock();
        UIManager.Instance.ToggleCrosshairVisibility();
        InputManager.InputTypeChanged -= InputChangedWhilstUIOpen;
    }

    public void DriveSelected(DataDrive drive)
    {
        UnlockPlayer();
        LoadDrive(drive);
    }

    public void UnloadDrive()
    {
        PlayerInventory.Instance.dataDrivesHeld.Add(insertedDrive);
        insertedDrive = null;
        insertedDriveName.text = NODISK;

        if (textOutput != null) textOutput.text = string.Empty;
        if (outputScreen != null)
        {
            defaultScreen.SetActive(true);
            outputScreen.SetActive(false);
        }
        if (audioOutput != null) audioOutput.DiskRemoved();
        if (anim != null) anim.SetTrigger("Eject");
        SoundManager.Instance.PlaySFXOneShot(audioOutput == null ? ejectSfx : audioDiscSfx, 0, 0.3f);
        if (visiblePD != null) visiblePD.SetActive(false);
    }

    private void LoadDrive(DataDrive drive)
    {
        insertedDrive = drive;
        PlayerInventory.Instance.dataDrivesHeld.Remove(drive);
        insertedDriveName.text = insertedDrive.DiskName;
        if (anim != null) anim.SetTrigger("Insert");
        SoundManager.Instance.PlaySFXOneShot(audioOutput == null ? insertSfx : audioDiscSfx, 0, 0.3f);
        if (visiblePD != null) visiblePD.SetActive(true);
    }
}
