using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    [SerializeField] private Material offMaterial;
    [SerializeField] private Material onMaterial;
    [SerializeField] private Renderer lightRenderer;
    [SerializeField] private Light lightSource;

    [Range(1f, 20f)]
    [SerializeField] private float minFlickersPerSecond;
    [Range(0.01f, 0.1f)]
    [SerializeField] private float flickerDuration;

    private float flickerTimer = 0f;
    private float timeTillNextFlicker = 0f;

    private float onTimer = 0f;
    private bool isOn = false;

    private void Start()
    {
        timeTillNextFlicker = Random.Range(0.01f, 1f / minFlickersPerSecond);
        lightRenderer.material = offMaterial;
        lightSource.enabled = false;
    }

    private void Update()
    {
        if (isOn)
        {
            onTimer += Time.deltaTime;
            if (onTimer > flickerDuration)
            {
                onTimer = 0f;
                flickerTimer = 0f;
                timeTillNextFlicker = Random.Range(0.01f, 1f / minFlickersPerSecond);
                lightRenderer.material = offMaterial;
                lightSource.enabled = false;
                isOn = false;
            }
        }
        else
        {
            flickerTimer += Time.deltaTime;
            if (flickerTimer >= timeTillNextFlicker)
            {
                lightRenderer.material = onMaterial;
                lightSource.enabled = true;
                isOn = true;
            }
        }
    }
}
