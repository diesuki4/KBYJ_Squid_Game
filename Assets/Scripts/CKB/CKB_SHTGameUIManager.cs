using System;
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
        ShowAllDrawArea(show);
        ShowSugarHoneycombStarImage(show);
    }
    public void ShowCountDownText(bool show) { countDownText.gameObject.SetActive(show); }
    public void ShowDrawArea(Image area, bool show)
    {
        Color color = area.color;
        color.a = Convert.ToInt32(show);
        area.color = color;
    }
    public void ShowAllDrawArea(bool show)
    {
        foreach (RectTransform area in drawAreas)
            ShowDrawArea(area.GetComponent<Image>(), show);
    }
    public void ShowSugarHoneycombStarImage(bool show) { sugarHoneycombStarImage.gameObject.SetActive(show); }

    public Image GraphicRaycast(Vector2 mousePosition)
    {
        PointerEventData evtData = new PointerEventData(evtSystem);
        evtData.position = mousePosition;

        List<RaycastResult> rayResults = new List<RaycastResult>();
        Image hitImage = null;

        grpRaycaster.Raycast(evtData, rayResults);

        if (0 < rayResults.Count)
            hitImage = rayResults[0].gameObject.GetComponent<Image>();
        
        return hitImage;
    }

    public bool IsInnerArea(Image area)
    {
        if (area.name.Contains("Area"))
            return true;
        else
            return false;
    }

    public bool IsAllDrawAreaVisible()
    {
        int numVisibleArea = 0;

        foreach (RectTransform rctArea in drawAreas)
            numVisibleArea += Convert.ToInt32(rctArea.GetComponent<Image>().color.a);

        return numVisibleArea == drawAreas.childCount;
    }
}
