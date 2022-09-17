using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CKB_MarbleGameUIManager : MonoBehaviour
{
    public static CKB_MarbleGameUIManager Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    Text countDownText;
    Text currentRoundText;
    Text userMarbleText;
    Text answerMarbleText;
    Text resultText;
    Text selectedText;
    RectTransform handRockPanel;
    RectTransform handPaperPanel;
    RectTransform marbleImages;
    Button evenButton;
    Button oddButton;

    void Start()
    {
        countDownText = transform.Find("Count Down Text").GetComponent<Text>();
        currentRoundText = transform.Find("Current Round Text").GetComponent<Text>();
        userMarbleText = transform.Find("User Marble Panel/User Marble Text").GetComponent<Text>();
        answerMarbleText = transform.Find("Answer Marble Text").GetComponent<Text>();
        resultText = transform.Find("Result Text").GetComponent<Text>();
        selectedText = transform.Find("Selected Text").GetComponent<Text>();
        handRockPanel = transform.Find("Hand Rock Panel").GetComponent<RectTransform>();
        handPaperPanel = transform.Find("Hand Paper Panel").GetComponent<RectTransform>();
        marbleImages = handPaperPanel.Find("Marble Images").GetComponent<RectTransform>();
        evenButton = transform.Find("Even Button").GetComponent<Button>();
        oddButton = transform.Find("Odd Button").GetComponent<Button>();

        handRockPanel.anchoredPosition = Vector2.up * 500;
        handPaperPanel.anchoredPosition = Vector2.zero;
    }

    void Update() { }

    public void SetCountDownText(float seconds)
    {
        countDownText.text = Mathf.CeilToInt(seconds).ToString();
    }

    public void SetCurrentRoundText(int currentRound, int totalRound)
    {
        currentRoundText.text = "현재 라운드 " + currentRound + " / " + totalRound;
    }

    public void SetUserMarbleText(int count)
    {
        userMarbleText.text = "당신의 구슬 : " + count + "개";
    }

    public void SetAnswerMarbleText(int count)
    {
        answerMarbleText.text = "정답 개수 : " + count;
    }

    public void SetResultText(bool result)
    {
        resultText.text = result ? "맞았습니다!" : "틀렸습니다!";
    }

    public void SetSelectedText(int selection)
    {
        selectedText.text = (selection == -1) ? "" : (selection == 0) ? "선택 : 짝수" : "선택 : 홀수";
    }

    public void SetMarbleImages(int marbleCount)
    {
        for (int i = 0; i < marbleImages.childCount; ++i)
            if (i < marbleCount)
                marbleImages.GetChild(i).gameObject.SetActive(true);
            else
                marbleImages.GetChild(i).gameObject.SetActive(false);
    }

    public void OnEvenOddButtonClick(int selection)
    {
        CKB_MarbleGameManager.Instance.selection = selection;

        SetSelectedText(selection);
    }

    public void ShowAllUI(bool show)
    {
        ShowCountDownText(show);
        ShowCurrentRoundText(show);
        ShowUserMarbleText(show);
        ShowAnswerMarbleText(show);
        ShowResultText(show);
        ShowSelectedText(show);
        ShowHandRockPanel(show);
        ShowHandPaperPanel(show);
        ShowEvenOddButton(show);
    }
    public void ShowCountDownText(bool show) { countDownText.gameObject.SetActive(show); }
    public void ShowCurrentRoundText(bool show) { currentRoundText.gameObject.SetActive(show); }
    public void ShowUserMarbleText(bool show) { userMarbleText.transform.parent.gameObject.SetActive(show); }
    public void ShowAnswerMarbleText(bool show) { answerMarbleText.gameObject.SetActive(show); }
    public void ShowResultText(bool show) { resultText.gameObject.SetActive(show); }
    public void ShowSelectedText(bool show) { selectedText.gameObject.SetActive(show); }
    public void ShowHandRockPanel(bool show) { handRockPanel.gameObject.SetActive(show); }
    public void ShowHandPaperPanel(bool show) { handPaperPanel.gameObject.SetActive(show); }
    public void ShowEvenOddButton(bool show) { evenButton.gameObject.SetActive(show); oddButton.gameObject.SetActive(show); }

    public void AppearHandPanel(int marbleCount, float destY = 0f, float duration = 1f)
    {
        handRockPanel.anchoredPosition = Vector2.up * 500;
        ShowHandRockPanel(true);

        handRockPanel.DOAnchorPosY(destY, duration).SetEase(Ease.OutElastic).OnComplete(() =>
        {
            ShowHandRockPanel(false);
            SetMarbleImages(marbleCount);
            handPaperPanel.anchoredPosition = Vector2.zero;
            ShowHandPaperPanel(true);
        });
    }

    public void DisappearHandPanel(float duration = 1f, float destY = 500f)
    {
        handPaperPanel.DOAnchorPosY(destY, duration).OnComplete(() =>
        {
            ShowHandPaperPanel(false);
        });
    }

    public void BlinkResultTextDuring(CKB_MarbleGameManager.State state, float resultBlinkTerm = 0.2f)
    {
        StartCoroutine(IEBlinkResultTextDuring(state, resultBlinkTerm));
    }

    IEnumerator IEBlinkResultTextDuring(CKB_MarbleGameManager.State state, float resultBlinkTerm)
    {
        while (CKB_MarbleGameManager.Instance.state == state)
        {
            if (resultText.gameObject.activeSelf)
                ShowResultText(false);
            else
                ShowResultText(true);

            yield return new WaitForSeconds(resultBlinkTerm);
        }
    }
}
