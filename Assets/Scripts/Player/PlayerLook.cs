using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera cam;
    private float xRotation = 0f;

    public float xSensitivity = 30f;
    public float ySensitivity = 30f;

    private float currentShakeTimer = 0f;
    private float timeToShake = 0f;
    private bool isShaking = false;
    private float cameraShakeIntensity = 5f;

    private bool lookLocked = false;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ProcessLook(Vector2 input)
    {
        if (lookLocked) return;

        float mouseX = input.x;
        float mouseY = input.y;

        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        float camZ = 0f;
        if(isShaking)
        {
            camZ = Random.Range(-cameraShakeIntensity, cameraShakeIntensity);
            currentShakeTimer += Time.deltaTime;
            if(currentShakeTimer > timeToShake)
            {
                isShaking = false;
            }
        }


        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, camZ);

        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    }

    public void ToggleLookLock(bool resetCamera = true)
    {
        lookLocked = !lookLocked;
        if(resetCamera)
        {
            cam.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void CameraShake(float timer, float intensity = 5f)
    {
        cameraShakeIntensity = intensity;
        timeToShake = timer;
        currentShakeTimer = 0f;
        isShaking = true;
    }
}
