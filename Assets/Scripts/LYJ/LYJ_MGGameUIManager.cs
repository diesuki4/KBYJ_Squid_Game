using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/* UI를 설정하는데 필요한 함수 제공 */
public class LYJ_MGGameUIManager : MonoBehaviour
{
    public static LYJ_MGGameUIManager Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public TextMeshProUGUI countDownText;
    public GameObject mugunghwa;
    public GameObject bloom;
    public GameObject crosshair;

    void Start()
    {
        
    }

    void Update() { }

    // 1 내용 적용 / 수정
    public void SetCountDownText(float time)
    {
        // 3.00001 ~ 3.99999 -> 4
        countDownText.text = Mathf.CeilToInt(time).ToString();
    }

    // 2 전부 turn on / off
    public void ShowAllUI(bool show)
    {
        ShowCountDownText(show);
        ShowMugunghwa(show);
        ShowBloom(show);
        ShowCrosshair(show);
    }
    
    // + 타이머 제외 전부 on /off
    public void ShowAllUITres(bool show)
    {
        ShowMugunghwa(show);
        ShowBloom(show);
        ShowCrosshair(show);
    }
    
    // 3 개별 turn on / off
    public void ShowCountDownText(bool show) { countDownText.gameObject.SetActive(show); }
    
    public void ShowMugunghwa(bool show) { mugunghwa.SetActive(show); }
    
    public void ShowBloom(bool show) { bloom.SetActive(show); }

    public void ShowCrosshair(bool show)
    {
        crosshair.SetActive(show);
    }
}
