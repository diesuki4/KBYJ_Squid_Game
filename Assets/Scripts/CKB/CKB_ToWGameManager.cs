using System.Linq;
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
    [Header("시작 시 줄에 끌려가는 시간")]
    public float fixPlayerDuratoin;
    [Header("시작 시 줄에 고정되는 속도")]
    public float fixPlayerSpeed;
    [Header("클릭 당 점수")]
    public int clickScore;
    [Header("게임이 종료되는 점수 차이")]
    public int maxDiffScore;

    public CKB_Player player;

    int ourScore;
    int opponentScore;
    float currentTime;
    Transform closestLineBone;
    Vector3 fixDestPos;
    Quaternion fixDestRot;
    float heightDiffBtwLineNPlayer;

    Transform line;
    Vector3 startLinePos;
    Vector3 endLinePos;

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
            player.state = CKB_Player.State.Stop;
        }

        closestLineBone = line.GetComponent<CKB_LinePositionUpdater>().ClosestBone(player.transform.position);

        heightDiffBtwLineNPlayer = closestLineBone.position.y - player.transform.position.y;

        fixDestPos = closestLineBone.position + Vector3.down * heightDiffBtwLineNPlayer;
        fixDestRot = Quaternion.LookRotation(closestLineBone.forward);

        state = State.FixPlayer;
    }

    void UpdateFixPlayer()
    {
        currentTime += Time.deltaTime;

        if (currentTime < fixPlayerDuratoin)
        {
            player.transform.position = Vector3.Lerp(player.transform.position, fixDestPos, Time.deltaTime * fixPlayerSpeed);
            player.transform.rotation = Quaternion.Lerp(player.transform.rotation, fixDestRot, Time.deltaTime * fixPlayerSpeed);
        }
        else
        {
            if (CKB_GameManager.Instance.debugMode)
            {
                Debug.Log("[CKB_ToWGameManager] 최종 위치 고정하기");
                player.transform.position = fixDestPos;
                player.transform.rotation = fixDestRot;
            }

            CKB_ToWGameUIManager.Instance.SetOurScoreText(ourScore);
            CKB_ToWGameUIManager.Instance.SetOpponentScoreText(opponentScore);
            CKB_ToWGameUIManager.Instance.ShowAllUI(true);

            LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
            startLinePos = lineRenderer.GetPosition(0);
            endLinePos = lineRenderer.GetPosition(lineRenderer.positionCount - 1);

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
            if (player.state == CKB_Player.State.Die)
                return;

            player.transform.position = closestLineBone.position + Vector3.down * heightDiffBtwLineNPlayer;
            player.transform.rotation = Quaternion.LookRotation(closestLineBone.forward);

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

                Vector3 destLinePos = Vector3.Lerp(startLinePos, endLinePos, (float)(opponentScore - ourScore + maxDiffScore / 2) / maxDiffScore);
                line.position = Vector3.Lerp(line.position, destLinePos, Time.deltaTime);
            }
        }
        else
        {
            state = State.Result;
        }
    }

    void UpdateResult()
    {
        bool result = opponentScore <= ourScore;

        CKB_ToWGameUIManager.Instance.ShowAllUI(false);

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
}
