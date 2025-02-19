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
    private bool ascendingIntensity = false;
    private float cameraShakeIntensity = 5f;

    private bool isDescendingShake = false;
    private float timeToDescend = 0f;

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

        xRotation -= mouseY * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

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

        transform.Rotate(Vector3.up * mouseX * xSensitivity);
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
