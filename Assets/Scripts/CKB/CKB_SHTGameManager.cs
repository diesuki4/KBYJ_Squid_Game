using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKB_SHTGameManager : MonoBehaviour
{
    public static CKB_SHTGameManager Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    enum State
    {
        Idle = 1,
        Conversation = 2,
        Initialize = 4,
        InGame = 8,
        Result = 16,
        End = 32,
        Die = 64
    }
    State state;

    [Header("달고나 게임 시간")]
    public float inGameTime;

    float currentTime;

    void Start()
    {
        state = State.Idle;
    }

    void Update()
    {
        switch (state)
        {
            case State.Idle :
                UpdateIdle();
                break;
            case State.Conversation :
                break;
            case State.Initialize :
                UpdateInitialize();
                break;
            case State.InGame :
                UpdateInGame();
                break;
            case State.Result :
                UpdateResult();
                break;
            case State.End :
                UpdateEnd();
                break;
            case State.Die :
                UpdateDie();
                break;
        }
    }

    void UpdateIdle()
    {
        CKB_SHTGameUIManager.Instance.ShowAllUI(false);

        CKB_UI_TextDialogue.Instance.onStart = () => { state = State.Conversation; };
        CKB_UI_TextDialogue.Instance.AppearTextDialogue();
        CKB_UI_TextDialogue.Instance.EnqueueConversationText("달고나 게임에 오신 것을 환영합니다!");
        CKB_UI_TextDialogue.Instance.EnqueueConversationText("선을 따라 달고나를 자르세요!");
        CKB_UI_TextDialogue.Instance.EnqueueConversationText("달고나가 깨지면 탈락하게 됩니다.");
        CKB_UI_TextDialogue.Instance.DisappearTextDialogue();
        CKB_UI_TextDialogue.Instance.onComplete = () => { state = State.Initialize; };
    }

    void UpdateInitialize()
    {
        CKB_SHTGameUIManager.Instance.ShowAllUI(true);
        CKB_SHTGameUIManager.Instance.ShowDrawAreas(false);

        state = State.InGame;
    }

    void UpdateInGame()
    {
        currentTime += Time.deltaTime;

        if (currentTime < inGameTime)
        {
            CKB_SHTGameUIManager.Instance.SetCountDownText(inGameTime - currentTime);
            CKB_SHTGameUIManager.Instance.ProcessLineDraw();
        }
        else
        {
            state = State.Result;
        }
    }

    void UpdateResult()
    {/*
        CKB_ToWGameUIManager.Instance.ShowAllUI(false);

        bool result = opponentScore <= ourScore;

        CKB_UI_TextDialogue.Instance.onStart = () => { state = State.Conversation; };
        CKB_UI_TextDialogue.Instance.AppearTextDialogue();
        CKB_UI_TextDialogue.Instance.EnqueueConversationText("게임이 종료되었습니다.");
        CKB_UI_TextDialogue.Instance.DisappearTextDialogue();
        CKB_UI_TextDialogue.Instance.onComplete = () => { state = (result) ? State.Alive : State.Die; };*/
    }

    void UpdateAlive()
    {
        if (CKB_GameManager.Instance.debugMode)
            Debug.Log("[CKB_SHTGameManager] 상대편 발판 떨어트리기");

        state = State.End;
    }

    void UpdateEnd()
    {
        if (CKB_GameManager.Instance.debugMode)
            Debug.Log("[CKB_SHTGameManager] 우리 편 발판 떨어트리기");

        state = State.End;
    }

    void UpdateDie()
    {
        if (CKB_GameManager.Instance.debugMode)
            Debug.Log("[CKB_SHTGameManager] 우리 편 발판 떨어트리기");

        state = State.End;
    }
}
