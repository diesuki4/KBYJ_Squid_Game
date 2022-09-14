using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKB_ToWGameManager : MonoBehaviour
{
    public static CKB_ToWGameManager Instance;

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
        FixPlayer = 8,
        Pulling = 16,
        Result = 32,
        Alive = 64,
        Die = 128,
        End = 256
    }
    State state;

    [Header("줄다리기 시간")]
    public float pullTime;
    [Header("초기화 시간")]
    public float initializeDuratoin;
    [Header("시작 시 줄에 고정되는 속도")]
    public float fixSpeed;
    [Header("클릭 당 점수")]
    public int clickScore;

    int ourScore;
    int opponentScore;
    float currentTime;
    Vector3 lineDestPos;
    Quaternion lineDestRot;

    Transform line;

    void Start()
    {
        line = GameObject.Find("Line").transform;

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
            case State.FixPlayer :
                UpdateFixPlayer();
                break;
            case State.Pulling :
                UpdatePulling();
                break;
            case State.Result :
                UpdateResult();
                break;
            case State.Alive :
                UpdateAlive();
                break;
            case State.Die :
                UpdateDie();
                break;
            case State.End :
                break;
        }
    }

    void UpdateIdle()
    {
        CKB_ToWGameUIManager.Instance.ShowAllUI(false);

        CKB_UI_TextDialogue.Instance.onStart = () => { state = State.Conversation; };
        CKB_UI_TextDialogue.Instance.AppearTextDialogue();
        CKB_UI_TextDialogue.Instance.EnqueueConversationText("이번 게임은 줄다리기입니다.");
        CKB_UI_TextDialogue.Instance.EnqueueConversationText("버튼을 눌러 상대편을 떨어트리면 승리하게 됩니다.");
        CKB_UI_TextDialogue.Instance.EnqueueConversationText("행운을 빕니다.");
        CKB_UI_TextDialogue.Instance.EnqueueConversationText("강한 팀이 승리할 수 있습니다.");
        CKB_UI_TextDialogue.Instance.DisappearTextDialogue();
        CKB_UI_TextDialogue.Instance.onComplete = () => { state = State.Initialize; };
    }

    void UpdateInitialize()
    {
        if (CKB_GameManager.Instance.debugMode)
        {
            Debug.Log("[CKB_ToWGameManager] 이동 제한하기");
            CKB_Player.Instance.state = CKB_Player.State.Stop;
        }

        Vector3 destVec = line.position - CKB_Player.Instance.transform.position;

        destVec = Vector3.ProjectOnPlane(destVec, line.forward);

        lineDestPos = CKB_Player.Instance.transform.position + destVec;
        lineDestRot = Quaternion.LookRotation(line.forward);

        state = State.FixPlayer;
    }

    void UpdateFixPlayer()
    {
        currentTime += Time.deltaTime;

        if (currentTime < initializeDuratoin)
        {
            CKB_Player player = CKB_Player.Instance;

            player.transform.position = Vector3.Lerp(player.transform.position, lineDestPos, Time.deltaTime * fixSpeed);
            player.transform.rotation = Quaternion.Lerp(player.transform.rotation, lineDestRot, Time.deltaTime * fixSpeed);
        }
        else
        {
            if (CKB_GameManager.Instance.debugMode)
            {
                Debug.Log("[CKB_ToWGameManager] 최종 위치 고정하기");
                CKB_Player.Instance.transform.position = lineDestPos;
                CKB_Player.Instance.transform.rotation = lineDestRot;

                Debug.Log("[CKB_ToWGameManager] 줄의 자식으로 등록하기");
                CKB_Player.Instance.transform.SetParent(line);
            }

            CKB_ToWGameUIManager.Instance.SetOurScoreText(ourScore);
            CKB_ToWGameUIManager.Instance.SetOpponentScoreText(opponentScore);
            CKB_ToWGameUIManager.Instance.ShowAllUI(true);

            currentTime = 0;

            state = State.Pulling;
        }
    }

    void UpdatePulling()
    {
        currentTime += Time.deltaTime;

        CKB_ToWGameUIManager.Instance.SetCountDownText(pullTime - currentTime);

        if (currentTime < pullTime)
        {
            if (CKB_GameManager.Instance.debugMode)
            {
                if (Input.GetKeyDown(KeyCode.LeftBracket))
                {
                    CKB_ToWGameUIManager.Instance.SetOurScoreText(ourScore += clickScore);
                    Debug.Log("[CKB_ToWGameManager] 우리 편 점수 증가 : " + ourScore);
                }
                if (Input.GetKeyDown(KeyCode.RightBracket))
                {
                    CKB_ToWGameUIManager.Instance.SetOpponentScoreText(opponentScore += clickScore);
                    Debug.Log("[CKB_ToWGameManager] 상대편 점수 증가 : " + opponentScore);
                }
            }
        }
        else
        {
            state = State.Result;
        }
        
    }

    void UpdateResult()
    {
        CKB_ToWGameUIManager.Instance.ShowAllUI(false);

        bool result = opponentScore <= ourScore;

        CKB_UI_TextDialogue.Instance.onStart = () => { state = State.Conversation; };
        CKB_UI_TextDialogue.Instance.AppearTextDialogue();
        CKB_UI_TextDialogue.Instance.EnqueueConversationText("게임이 종료되었습니다.");
        CKB_UI_TextDialogue.Instance.EnqueueConversationText((result ? "우리 " : "상대") + "편이 승리하였습니다.");
        if (!result) CKB_UI_TextDialogue.Instance.EnqueueConversationText("아쉽군요.");
        CKB_UI_TextDialogue.Instance.DisappearTextDialogue();
        CKB_UI_TextDialogue.Instance.onComplete = () => { state = (result) ? State.Alive : State.Die; };
    }

    void UpdateAlive()
    {
        if (CKB_GameManager.Instance.debugMode)
            Debug.Log("[CKB_ToWGameManager] 상대편 발판 떨어트리기");

        state = State.End;
    }

    void UpdateDie()
    {
        if (CKB_GameManager.Instance.debugMode)
            Debug.Log("[CKB_ToWGameManager] 우리 편 발판 떨어트리기");

        state = State.End;
    }

    void DetectGround(Vector3 position)
    {

    }
}
