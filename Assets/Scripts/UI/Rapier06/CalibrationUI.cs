using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CalibrationUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI locationName;
    [SerializeField] private Image locationBG;
    [SerializeField] private TextMeshProUGUI damageNumber;
    [SerializeField] private RectTransform map;
    [SerializeField] private RectTransform mapMask;
    [SerializeField] private RectTransform crosshair;

    [SerializeField] private RectTransform playerMarker;
    [SerializeField] private RectTransform targetMarker;

    [SerializeField] private GameObject chevronR;
    [SerializeField] private GameObject chevronL;
    [SerializeField] private GameObject chevronU;
    [SerializeField] private GameObject chevronD;

    [SerializeField] private List<RectTransform> locations;
    [SerializeField] private RectTransform targetlocation;
    [SerializeField] private Image completionBar;

    [SerializeField] private float mapMoveSpeed = 10f;
    [SerializeField] private float crosshairSnapDuration = 1f;

    [SerializeField] private AudioClip locationSnapSound;

    private Vector2 maxMapPosition;
    private RectTransform currentLocation;
    private const string NOLOCATION = "???";
    private bool crosshairSnapping = false;

    private float targetStay = 2f;
    [Header("Target Minigame Settings")]
    public float targetSnap = 0.1f;
    public float targetStayMin = 0.2f;
    public float targetStayMax = 1f;
    public float targetJitter = 0.5f;
    public float targetMinMove = 30f;
    public float targetMaxMove = 200f;

    private float targetCounter = 0f;
    private bool targetInPlace = false;
    private Vector3 targetStart;
    private Vector3 targetEnd;

    public float depletionRate = 0.01f;
    public float addRate = 0.03f;
    public float completionValue = 0.2f;


    private void Start()
    {
        maxMapPosition = new Vector2(
            map.sizeDelta.x/2 - mapMask.sizeDelta.x/2,
            map.sizeDelta.y/2 - mapMask.sizeDelta.y/2    
        );
    }

    private void Update()
    {
        UpdateDamageNumber();
        UpdateLocationName();
        UpdateChevrons();
        UpdateCrosshairColor();
    }

    public void UpdateCalibrationMinigame()
    {
        UpdatePlayerMarker();

        if (!targetInPlace)
        {
            targetStart = targetMarker.localEulerAngles;
            targetStay = Random.Range(targetStayMin, targetStayMax);
            targetCounter = 0f;
            targetInPlace = true;
            float currentRot = targetMarker.localEulerAngles.z;
            targetEnd = new Vector3(0, 0, targetStart.z + Random.Range(30f, 240f));
        }
        else
        {
            targetCounter += Time.deltaTime;
            if (targetCounter > targetStay)
            {
                targetInPlace = false;
            }
            else if (targetCounter < targetSnap)
            {
                targetMarker.localEulerAngles = Vector3.Lerp(targetStart, targetEnd, targetCounter / targetSnap);
            } else
            {
                targetMarker.localEulerAngles += new Vector3(0, 0, Random.Range(-targetJitter, targetJitter));
            }
        }

        completionValue -= (depletionRate / 1000f);
        completionValue = Mathf.Clamp(completionValue, 0, 1);
        completionBar.fillAmount = completionValue;
    }

    private void UpdatePlayerMarker()
    {
        Vector2 localMousePos = UIManager.Instance.GetComponent<RectTransform>().InverseTransformPoint(Input.mousePosition);
        Vector2 direction = localMousePos - (Vector2)playerMarker.localPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        playerMarker.localRotation = Quaternion.Euler(0, 0, angle);

        float matchValue = Mathf.Abs(playerMarker.localEulerAngles.z - targetMarker.localEulerAngles.z);
        playerMarker.GetComponentInChildren<Image>().color = matchValue < 20f ? UIColors.terminalGreen : UIColors.terminalRed;
        if (matchValue < 20f) completionValue += (addRate / 1000f);
    }

    public void CheckCrosshairSnapping(Vector2 currentInput)
    {
        if(currentInput == Vector2.zero && currentLocation != null)
        {
            if(!crosshairSnapping)
            {
                crosshairSnapping = true;
                StartCoroutine(LerpToLocation());
            }
        } else
        {
            StopAllCoroutines();
            crosshairSnapping = false;
        }
    }

    private IEnumerator LerpToLocation()
    {
        Vector2 start = map.anchoredPosition;
        Vector2 move = CanvasPosition(crosshair) - CanvasPosition(currentLocation);
        Vector2 end = start+move;
        float elapsed = 0f;

        while (elapsed < crosshairSnapDuration)
        {
            float t = elapsed / crosshairSnapDuration;
            map.anchoredPosition = Vector2.Lerp(start, end, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        map.anchoredPosition = end;
    }

    private void UpdateCrosshairColor()
    {
        crosshair.GetComponent<Image>().color = currentLocation != null ? UIColors.terminalRed : UIColors.white;
    }
    
    private void UpdateDamageNumber()
    {
        List<Vector2> mapCorners = new List<Vector2>();
        mapCorners.Add(new Vector2(map.sizeDelta.x / 2, map.sizeDelta.y / 2));
        mapCorners.Add(new Vector2(-map.sizeDelta.x / 2, map.sizeDelta.y / 2));
        mapCorners.Add(new Vector2(map.sizeDelta.x / 2, -map.sizeDelta.y / 2));
        mapCorners.Add(new Vector2(-map.sizeDelta.x / 2, -map.sizeDelta.y / 2));
        float maxDist = 0f;
        foreach (var corner in mapCorners)
        {
            float d = Vector2.Distance(CanvasPosition(targetlocation), corner);
            if (d > maxDist) maxDist = d;
        }

        float distToTarget = Vector2.Distance(CanvasPosition(targetlocation), CanvasPosition(crosshair)) /maxDist;
        float damage = Mathf.Exp((1 - distToTarget) * 13);
        string formattedNumber = "";
        if (damage > 1000000) formattedNumber = (damage / 1000000).ToString("0.#") + "m";
        else if (damage > 1000) formattedNumber = (damage / 1000).ToString("0.#") + "k";
        else formattedNumber = damage.ToString("0.#");

        damageNumber.text = formattedNumber;
    }

    private void UpdateLocationName()
    {
        string name = NOLOCATION;
        bool locationMatched = false;
        foreach (var loc in locations)
        {
            if(Vector2.Distance(CanvasPosition(crosshair), CanvasPosition(loc)) < loc.sizeDelta.y/2)
            {
                name = loc.name;
                locationMatched = true;
                if (currentLocation == null) SoundManager.Instance.PlaySFXOneShot(locationSnapSound);
                currentLocation = loc;
                break;
            }
        }

        locationName.text = name;
        if(!locationMatched) currentLocation = null;
        locationBG.color = currentLocation == null ? UIColors.grey : currentLocation == targetlocation ? UIColors.terminalRed : UIColors.terminalGreen;

    }

    private void UpdateChevrons()
    {
        Vector2 targetVector = CanvasPosition(targetlocation) - CanvasPosition(crosshair);
        chevronR.SetActive(targetVector.x > mapMask.sizeDelta.x/2);
        chevronL.SetActive(targetVector.x < -mapMask.sizeDelta.x/2);
        chevronU.SetActive(targetVector.y > mapMask.sizeDelta.y/2);
        chevronD.SetActive(targetVector.y < -mapMask.sizeDelta.y/2);
    }

    private Vector2 CanvasPosition(RectTransform r)
    {
        return GetComponent<RectTransform>().InverseTransformPoint(r.position);
    }

    private Vector2 CanvasPosition(Vector3 r)
    {
        return GetComponent<RectTransform>().InverseTransformPoint(r);
    }

    public void MoveMap(CalibrationButton.ButtonDirection d)
    {
        float x = map.anchoredPosition.x;
        float y = map.anchoredPosition.y;
        switch(d)
        {
            case CalibrationButton.ButtonDirection.Left:
                x += mapMoveSpeed;
                break;
            case CalibrationButton.ButtonDirection.Right:
                x -= mapMoveSpeed;
                break;
            case CalibrationButton.ButtonDirection.Up:
                y -= mapMoveSpeed;
                break;
            case CalibrationButton.ButtonDirection.Down:
                y += mapMoveSpeed;
                break;
            default:
                break;
        }
        x = Mathf.Clamp(x, -maxMapPosition.x, maxMapPosition.x);
        y = Mathf.Clamp(y, -maxMapPosition.y, maxMapPosition.y);
        map.anchoredPosition = new Vector2(x, y);
    }

    public void MoveMap(Vector2 moveInput)
    {
        float x = Mathf.Clamp(map.anchoredPosition.x + (moveInput.x * mapMoveSpeed), -maxMapPosition.x, maxMapPosition.x);
        float y = Mathf.Clamp(map.anchoredPosition.y + (moveInput.y * mapMoveSpeed), -maxMapPosition.y, maxMapPosition.y);
        map.anchoredPosition = new Vector2(x, y);
    }
}
