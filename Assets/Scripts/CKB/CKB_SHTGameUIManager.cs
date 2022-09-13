using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    Text ourScoreText;
    Text opponentScoreText;
    Text ourSideText;
    Text pullText;
    Button pullButton;

    void Start()
    {
        countDownText = transform.Find("Count Down Text").GetComponent<Text>();
        ourScoreText = transform.Find("Our Score Panel/Our Score Text").GetComponent<Text>();
        opponentScoreText = transform.Find("Opponent Score Panel/Opponent Score Text").GetComponent<Text>();
        ourSideText = transform.Find("Our Side Text").GetComponent<Text>();
        pullText = transform.Find("Pull Text").GetComponent<Text>();
        pullButton = transform.Find("Pull Button").GetComponent<Button>();
    }

    void Update() { }

    public void SetCountDownText(float seconds)
    {
        countDownText.text = Mathf.CeilToInt(seconds).ToString();
    }

    public void SetOurScoreText(int score)
    {
        ourScoreText.text = score.ToString();
    }

    public void SetOpponentScoreText(int score)
    {
        opponentScoreText.text = score.ToString();
    }

    public void OnPullButtonClick()
    {
        if (CKB_GameManager.Instance.debugMode)
            Debug.Log("[CKB_TowGameUIManager] Pull 버튼 내용 구현 예정");
    }

    public void ShowAllUI(bool show)
    {
        ShowCountDownText(show);
        ShowOurScoreText(show);
        ShowOpponentScoreText(show);
        ShowOurSideText(show);
        ShowPullText(show);
        ShowPullButton(show);
    }
    public void ShowCountDownText(bool show) { countDownText.gameObject.SetActive(show); }
    public void ShowOurScoreText(bool show) { ourScoreText.transform.parent.gameObject.SetActive(show); }
    public void ShowOpponentScoreText(bool show) { opponentScoreText.transform.parent.gameObject.SetActive(show); }
    public void ShowOurSideText(bool show) { ourSideText.gameObject.SetActive(show); }
    public void ShowPullText(bool show) { pullText.gameObject.SetActive(show); }
    public void ShowPullButton(bool show) { pullButton.gameObject.SetActive(show); }
}
