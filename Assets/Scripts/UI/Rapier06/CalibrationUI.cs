using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CalibrationUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI locationName;
    [SerializeField] private TextMeshProUGUI damageNumber;
    [SerializeField] private RectTransform map;
    [SerializeField] private RectTransform mapMask;
    [SerializeField] private RectTransform crosshair;

    [SerializeField] private GameObject chevronR;
    [SerializeField] private GameObject chevronL;
    [SerializeField] private GameObject chevronU;
    [SerializeField] private GameObject chevronD;

    [SerializeField] private List<RectTransform> locations;
    [SerializeField] private RectTransform targetlocation;

    [SerializeField] private float mapMoveSpeed = 10f;

    private Vector2 maxMapPosition;

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
        damageNumber.text = "" + Mathf.Exp((1-distToTarget)*15);
    }

    private void UpdateLocationName()
    {
        string name = "???";
        foreach (var loc in locations)
        {
            if(Vector2.Distance(CanvasPosition(crosshair), CanvasPosition(loc)) < loc.sizeDelta.y/2)
            {
                name = loc.name;
                break;
            }
        }

        locationName.text = name;
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
