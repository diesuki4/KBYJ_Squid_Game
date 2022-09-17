using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LYJ_Bedtime : MonoBehaviour
{
    
    public float timeValue = 5;
    public TextMeshProUGUI timerText;

    /* 상태머신 */
    public enum State
    {
        Idle,
        Conversation,
        Bedtime,
        End,
    }

    public State state;
    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Idle:
                UpdateIdle();
                break;
            case State.Conversation:
                break;
            case State.Bedtime:
                UpdateBedtime();
                break;
            case State.End:
                UpdateEnd();
                break;
        }
    }

    private void UpdateIdle()
    {
        /* 대기화면 */
        // 게임 설명 (UI)
        // This is Red Light Green Light!
        // 제한 시간 내에 선 안으로 들어가면 통화입니다
        // Debug.Log("state = State.Idle");
        CKB_UI_TextDialogue.Instance.onStart = () => { state = State.Conversation; };
        CKB_UI_TextDialogue.Instance.AppearTextDialogue();
        CKB_UI_TextDialogue.Instance.EnqueueConversationText("취침 시간입니다.");
        CKB_UI_TextDialogue.Instance.EnqueueConversationText("취침시간 동안 다른 플레이어와 팀을 맺는 것이 좋습니다.");
        CKB_UI_TextDialogue.Instance.EnqueueConversationText("좋은밤 되세요.");
        CKB_UI_TextDialogue.Instance.DisappearTextDialogue();
        CKB_UI_TextDialogue.Instance.onComplete = () => { state = State.Bedtime; };
    }

    private void UpdateBedtime()
    {
        Timer();
    }

    private void UpdateEnd()
    {
        Debug.Log("state = State.End");
        // 다음 씬 로드
    }

    private void Timer()
    {
        if (timeValue > 0)
        {
            timeValue -= Time.deltaTime; // 프레임 == 60프레임에 1초 => 
            
        }
        else
        {
            timeValue += 150;
            // State.End로 변환
            LYJ_YeongHeeState lyjState = LYJ_YeongHee.Instance.GetComponent<LYJ_YeongHeeState>();
            lyjState.state = LYJ_YeongHeeState.State.End;
        }
        if (timeValue < 0)
        {
            timeValue = 0;
        }

        // Debug.Log((int)timeValue);
        timerText.text = string.Format("{000}", (int)timeValue);
    }
}
