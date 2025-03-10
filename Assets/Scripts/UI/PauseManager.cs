using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }
    [SerializeField] private int mainMenuSceneIndex;
    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }
    public void TogglePauseMenu()
    {
        if (PlayerMotor.Instance.movementOverridden) return; // Pausing only possible outside of focus interactablesto avoid keybind clash

        gameObject.SetActive(!gameObject.activeSelf);
        Time.timeScale = gameObject.activeSelf ? 0f : 1f;
        PlayerLook.Instance.lookLocked = gameObject.activeSelf;
        Cursor.lockState = gameObject.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;

        if (gameObject.activeSelf) SoundManager.Instance.PauseAllSound();
        else SoundManager.Instance.UnpauseAllPausedSound();
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneIndex);
    }
}
