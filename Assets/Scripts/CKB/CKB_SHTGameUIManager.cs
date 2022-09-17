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
    RectTransform toothpick;

    GraphicRaycaster grpRaycaster;
    EventSystem evtSystem;

    void Start()
    {
        countDownText = transform.Find("Count Down Text").GetComponent<Text>();
        drawAreas = transform.Find("Draw Areas").GetComponent<RectTransform>();
        sugarHoneycombStarImage = transform.Find("Sugar Honeycomb Star Image").GetComponent<Image>();
        toothpick = transform.Find("Toothpick").GetComponent<RectTransform>();

        sugarHoneycombStarImage.alphaHitTestMinimumThreshold = 0.5f;
        toothpick.Find("Toothpick Press").GetComponent<Image>().enabled = false;
        toothpick.Find("Toothpick Release").GetComponent<Image>().enabled = true;

        grpRaycaster = GetComponent<GraphicRaycaster>();
        evtSystem = GetComponent<EventSystem>();
    }

    void Update()
    {
        SetToothpickPosition(Input.mousePosition);
    }

    public void SetCountDownText(float seconds)
    {
        countDownText.text = Mathf.CeilToInt(seconds).ToString();
    }

    void SetToothpickPosition(Vector3 position)
    {
        toothpick.anchoredPosition = position;
    }

    public void ShowAllUI(bool show)
    {
        ShowCountDownText(show);
        ShowAllDrawArea(show);
        ShowSugarHoneycombStarImage(show);
        ShowToothpick(show);
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
    public void ShowToothpick(bool show) { toothpick.gameObject.SetActive(show); }

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
        if (area == sugarHoneycombStarImage)
            return false;
        else
            return true;
    }

    public bool IsAllDrawAreaVisible()
    {
        int numVisibleArea = 0;

        foreach (RectTransform rctArea in drawAreas)
            numVisibleArea += Convert.ToInt32(rctArea.GetComponent<Image>().color.a);

        return numVisibleArea == drawAreas.childCount;
    }
}
