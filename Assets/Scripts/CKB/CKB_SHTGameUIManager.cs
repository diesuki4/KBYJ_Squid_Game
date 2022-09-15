using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CKB_SHTGameUIManager : MonoBehaviour
{
    public static CKB_SHTGameUIManager Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    Text countDownText;
    RectTransform drawAreas;
    Image sugarHoneycombStarImage;

    GraphicRaycaster grpRaycaster;
    EventSystem evtSystem;

    void Start()
    {
        countDownText = transform.Find("Count Down Text").GetComponent<Text>();
        drawAreas = transform.Find("Draw Areas").GetComponent<RectTransform>();
        sugarHoneycombStarImage = transform.Find("Sugar Honeycomb Star Image").GetComponent<Image>();

        sugarHoneycombStarImage.alphaHitTestMinimumThreshold = 0.5f;

        grpRaycaster = GetComponent<GraphicRaycaster>();
        evtSystem = GetComponent<EventSystem>();
    }

    void Update() { }

    public void SetCountDownText(float seconds)
    {
        countDownText.text = Mathf.CeilToInt(seconds).ToString();
    }

    public void ShowAllUI(bool show)
    {
        ShowCountDownText(show);
        ShowDrawAreas(show);
        ShowSugarHoneycombStarImage(show);
    }
    public void ShowCountDownText(bool show) { countDownText.gameObject.SetActive(show); }
    public void ShowDrawAreas(bool show) { foreach (RectTransform rct in drawAreas) rct.gameObject.SetActive(show); }
    public void ShowSugarHoneycombStarImage(bool show) { sugarHoneycombStarImage.gameObject.SetActive(show); }

    public void ProcessLineDraw()
    {
        PointerEventData evtData = new PointerEventData(evtSystem);
        evtData.position = Input.mousePosition;

        List<RaycastResult> rayResults = new List<RaycastResult>();

        grpRaycaster.Raycast(evtData, rayResults);

        if (0 < rayResults.Count)
        {
            GameObject hitObj = rayResults[0].gameObject;
            print(hitObj.name);

            if (hitObj.name.Contains("Area"))
                if (!hitObj.activeSelf)
                    hitObj.SetActive(true);
        }
    }
}
