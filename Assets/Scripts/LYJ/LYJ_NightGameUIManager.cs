using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/* UI를 설정하는데 필요한 함수 제공 */
public class LYJ_NightGameUIManager : MonoBehaviour
{
    public static LYJ_NightGameUIManager Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public TextMeshProUGUI countDownText;
    public GameObject push;
    public GameObject baseball;

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
        ShowPush(show);
        ShowBaseball(show);
    }
    
    // + 타이머 제외 전부 on /off
    public void ShowAllUIDos(bool show)
    {
        ShowPush(show);
        ShowBaseball(show);
    }
    
    // 3 개별 turn on / off
    public void ShowCountDownText(bool show) { countDownText.gameObject.SetActive(show); }
    
    public void ShowPush(bool show) { push.SetActive(show); }
    
    public void ShowBaseball(bool show) { baseball.SetActive(show); }

}
