using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PDSelectUI : MonoBehaviour
{
    [SerializeField] private Transform PDUIButtonContainer;
    [SerializeField] private Button PDCancelBtn;
    [SerializeField] private GridLayoutGroup buttonGrid;
    private List<Button> pdButtons = new();

    public delegate void OnPDSelected(DataDrive drive);
    public static event OnPDSelected PDSelected;

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
        Cursor.lockState = CursorLockMode.Locked;
        PlayerMotor.Instance.TogglePlayerLock();
        PlayerLook.Instance.ToggleLookLock();
        UIManager.Instance.ToggleCrosshairVisibility();
        InputManager.InputTypeChanged -= InputChangedWhilstUIOpen;

        gameObject.SetActive(false);
    }

    public void ShowUI(Action<IServerDataObject> onClick, List<IServerDataObject> inventory)
    {
        if (!InputManager.Instance.GamepadIsCurrentInput()) Cursor.lockState = CursorLockMode.None;
        InputManager.InputTypeChanged += InputChangedWhilstUIOpen;
        PlayerMotor.Instance.TogglePlayerLock();
        PlayerLook.Instance.ToggleLookLock();
        UIManager.Instance.ToggleCrosshairVisibility();

        foreach (Transform t in PDUIButtonContainer) Destroy(t.gameObject);

        pdButtons.Clear();
        foreach (IServerDataObject d in inventory)
        {
            Button b = Instantiate(Resources.Load<Button>("PDButton"), PDUIButtonContainer);
            b.onClick.AddListener(UnlockPlayer);
            b.GetComponent<PDButton>().SetDrive(d, onClick);

            pdButtons.Add(b);
            if (inventory.IndexOf(d) == 0 && InputManager.Instance.GamepadIsCurrentInput()) b.Select();
        }

        foreach (Button b in pdButtons)
        {
            int index = pdButtons.IndexOf(b);

            int l = buttonGrid.constraintCount; //grid row length

            Selectable up = index > (l-1) ? pdButtons[index - l] : null;
            Selectable down = index < pdButtons.Count - (l - index % l) ? pdButtons[index + l >= pdButtons.Count ? pdButtons.Count - 1 : index + l] : PDCancelBtn;
            Selectable left = index % l >= 1 ? pdButtons[index - 1] : null;
            Selectable right = index % l <= 1 && index < pdButtons.Count - 1 ? pdButtons[index + 1] : null;

            b.navigation = UIManager.Instance.CreateNewNavigation(up, down, left, right);
        }

        PDCancelBtn.navigation = UIManager.Instance.CreateNewNavigation(pdButtons.Count > 0 ? pdButtons[pdButtons.Count - 1] : null, null, null, null);
    }
}
