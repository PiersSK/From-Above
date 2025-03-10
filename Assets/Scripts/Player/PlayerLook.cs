using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public static PlayerLook Instance {  get; private set; }

    public Camera cam;
    private float xRotation = 0f;

    [SerializeField] private MenuSlider mouseSensitivy;
    [SerializeField] private MenuSlider controllerXSensitivity;
    [SerializeField] private MenuSlider controllerYSensitivity;

    private float currentShakeTimer = 0f;
    private float timeToShake = 0f;
    private bool isShaking = false;
    private bool ascendingIntensity = false;
    private float cameraShakeIntensity = 5f;

    private bool isDescendingShake = false;
    private float timeToDescend = 0f;

    public bool lookLocked = false;

    private void Awake()
    {
        Instance = this;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ProcessLook(Vector2 input)
    {
        if (lookLocked) return;

        bool isMouse = InputManager.Instance.lastInputType == InputManager.LastInputType.KeyboardMouse;
        float xSensitivity = isMouse ? mouseSensitivy.GetModifiedValue() : controllerXSensitivity.GetModifiedValue();
        float ySensitivity = isMouse ? mouseSensitivy.GetModifiedValue() : controllerYSensitivity.GetModifiedValue();

        float mouseX = input.x * xSensitivity;
        float mouseY = input.y * ySensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f); // up down
        transform.Rotate(Vector3.up * mouseX); // left right


        float camZ = 0f;
        if(isShaking)
        {
            float intensity = ascendingIntensity ? cameraShakeIntensity * (currentShakeTimer / timeToShake) : cameraShakeIntensity;
            camZ = Random.Range(-intensity, intensity);
            currentShakeTimer += Time.deltaTime;
            if(currentShakeTimer > timeToShake)
            {
                isShaking = false;
                isDescendingShake = true;
                currentShakeTimer = 0f;
            }
        } else if (isDescendingShake)
        {
            float intensity = cameraShakeIntensity - cameraShakeIntensity * (currentShakeTimer / timeToDescend);
            camZ = Random.Range(-intensity, intensity);
            currentShakeTimer += Time.deltaTime;
            if (currentShakeTimer > timeToDescend)
            {
                isDescendingShake = false;
                currentShakeTimer = 0f;
            }
        }


        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, camZ);

    }

    public void ToggleLookLock(bool resetCamera = true)
    {
        lookLocked = !lookLocked;
        if(resetCamera)
        {
            cam.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void CameraShake(float timer, float intensity = 5f, bool ascending = false)
    {
        cameraShakeIntensity = intensity;
        timeToShake = timer;
        timeToDescend = timer * 0.2f;
        ascendingIntensity = ascending;

        currentShakeTimer = 0f;
        isShaking = true;

    }
}
