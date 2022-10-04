using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class CKB_MarbleGameManager : MonoBehaviourPunCallbacks
{
    public static CKB_MarbleGameManager Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public enum State
    {
        Idle = 1,
        Conversation = 2,
        Initialize = 4,
        Selection = 8,
        AnswerMarbleCount = 16,
        Result = 32,
        Alive = 64,
        Die = 128,
        End = 256,
        AllEnd = 512,
    }
    [HideInInspector]
    public State state;

    [Header("구슬 선택 시간")]
    public float selectTime;
    [Header("유저 구슬 개수")]
    public int userMarbleCount;
    [Header("정답 구슬 표시 시간")]
    public float answerMarbleDuration;
    [Header("결과 텍스트 표시 시간")]
    public float resultDuration;
    [Header("총 라운드 수")]
    public int totalRound;
    [HideInInspector]
    public int selection;

    GameObject goPlayer;
    public CKB_Player player;
    
    int answerMarbleCount;
    int currentRound;
    float currentTime;

    Animator agentAnim;

    public Transform trRandom;
    public Transform trRandomAgent;

    int playerCount;
    int playerEndCount;
    bool isEnd;
    float uniqueValue;

    void Start()
    {
        goPlayer = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
        player = goPlayer.GetComponent<CKB_Player>();

        agentAnim = PhotonNetwork.Instantiate("Agent2", Vector3.zero, Quaternion.identity).GetComponent<Animator>();

        uniqueValue = Random.Range(float.MinValue, float.MaxValue);
        photonView.RPC("RpcRequestSetPlayerPosition", RpcTarget.MasterClient, uniqueValue);

        state = State.Idle;
    }

    [PunRPC]
    void RpcRequestSetPlayerPosition(float uniqueValue)
    {
        photonView.RPC("RpcSetPlayerPosition", RpcTarget.All, playerCount, uniqueValue);
        photonView.RPC("RpcSetPlayerCount", RpcTarget.Others, ++playerCount);
    }

    [PunRPC]
    void RpcSetPlayerPosition(int posIdx, float uniqueValue)
    {
        if (Mathf.Approximately(this.uniqueValue, uniqueValue))
        {
            goPlayer.transform.position = trRandom.GetChild(posIdx).position;
            goPlayer.transform.rotation = trRandom.GetChild(posIdx).rotation;

            agentAnim.transform.position = trRandomAgent.GetChild(posIdx).position;
            agentAnim.transform.rotation = trRandomAgent.GetChild(posIdx).rotation;
        }
    }

    [PunRPC]
    void RpcSetPlayerCount(int playerCount)
    {
        this.playerCount = playerCount;
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
            case State.Selection :
                UpdateSelection();
                break;
            case State.AnswerMarbleCount :
                UpdateAnswerMarbleCount();
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
                UpdateEnd();
                break;
            case State.AllEnd :
                break;
        }

        if (CKB_GameManager.Instance.debugMode)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                Debug.Log("[CKB_MarbleGameManager] 사망 치트");
                CKB_MarbleGameUIManager.Instance.SetUserMarbleText(userMarbleCount = -99);
            }
            else if (Input.GetKeyDown(KeyCode.O))
            {
                Debug.Log("[CKB_MarbleGameManager] 생존 치트");
                CKB_MarbleGameUIManager.Instance.SetUserMarbleText(userMarbleCount = 99);
            }
        }

        EndDetect();
    }

    void EndDetect()
    {
        if (PhotonNetwork.IsMasterClient && isEnd == false)
        {
            if (playerEndCount == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                if (player.GetComponent<CKB_Player>().state == CKB_Player.State.Die)
                {
                    PhotonNetwork.LeaveRoom();
                    PhotonNetwork.LeaveLobby();
                    PhotonNetwork.Disconnect();
                    Application.Quit();
                }
                else
                {
                    PhotonNetwork.LoadLevel("EndScene");
                }

                isEnd = true;
            }
        }
    }

    void UpdateIdle()
    {
        CKB_MarbleGameUIManager.Instance.ShowAllUI(false);

        CKB_UI_TextDialogue.Instance.onStart = () => { state = State.Conversation; };
        CKB_UI_TextDialogue.Instance.AppearTextDialogue();
        CKB_UI_TextDialogue.Instance.EnqueueConversationText("이번 게임은 구슬 놀이입니다.");
        CKB_UI_TextDialogue.Instance.EnqueueConversationText("이 게임은 구슬이 짝수 또는 홀수인지 추측하는 게임입니다.");
        CKB_UI_TextDialogue.Instance.EnqueueConversationText("잘못 맞히면 2개의 구슬을 잃고, 맞히면 구슬 1개를 얻습니다.");
        CKB_UI_TextDialogue.Instance.EnqueueConversationText("4개의 라운드를 통과하면, 통과입니다!");
        CKB_UI_TextDialogue.Instance.DisappearTextDialogue();
        CKB_UI_TextDialogue.Instance.onComplete = () => { state = State.Initialize; };
    }

    void UpdateInitialize()
    {
        CKB_MarbleGameUIManager.Instance.SetSelectedText(selection = -1);
        
        CKB_MarbleGameUIManager.Instance.SetCurrentRoundText(++currentRound, totalRound);
        CKB_MarbleGameUIManager.Instance.SetUserMarbleText(userMarbleCount);

        CKB_MarbleGameUIManager.Instance.ShowAllUI(true);
        CKB_MarbleGameUIManager.Instance.ShowAnswerMarbleText(false);
        CKB_MarbleGameUIManager.Instance.ShowResultText(false);
        CKB_MarbleGameUIManager.Instance.ShowHandRockPanel(false);
        CKB_MarbleGameUIManager.Instance.ShowHandPaperPanel(false);

        currentTime = 0;

        state = State.Selection;
    }

    void UpdateSelection()
    {
        currentTime += Time.deltaTime;

        CKB_MarbleGameUIManager.Instance.SetCountDownText(selectTime - currentTime);

        if (selectTime <= currentTime)
        {            
            answerMarbleCount = Random.Range(5, 15);

            CKB_MarbleGameUIManager.Instance.SetAnswerMarbleText(answerMarbleCount);
            CKB_MarbleGameUIManager.Instance.ShowAnswerMarbleText(true);
            CKB_MarbleGameUIManager.Instance.ShowCountDownText(false);
            CKB_MarbleGameUIManager.Instance.ShowEvenOddButton(false);
            CKB_MarbleGameUIManager.Instance.AppearHandPanel(answerMarbleCount);

            currentTime = 0;

            state = State.AnswerMarbleCount;
        }
    }

    void UpdateAnswerMarbleCount()
    {
        currentTime += Time.deltaTime;

        if (answerMarbleDuration <= currentTime)
        {
            bool result = (answerMarbleCount % 2 == selection);

            currentTime = 0;

            state = State.Result;

            if (!result) 
                agentAnim.SetTrigger("Load");

            userMarbleCount += (result) ? 1 : -2;
            userMarbleCount = Mathf.Clamp(userMarbleCount, 0, int.MaxValue);
            CKB_MarbleGameUIManager.Instance.SetUserMarbleText(userMarbleCount);

            CKB_MarbleGameUIManager.Instance.SetResultText(result);
            CKB_MarbleGameUIManager.Instance.BlinkResultTextDuring(state);

            CKB_MarbleGameUIManager.Instance.DisappearHandPanel();
        }
    }

    void UpdateResult()
    {
        if (resultDuration <= currentTime)
            if (userMarbleCount <= 0)
                state = State.Die;
            else if (totalRound <= currentRound)
                state = State.Alive;
            else
                state = State.Initialize;
        else
            currentTime += Time.deltaTime;
    }

    void UpdateAlive()
    {
        CKB_MarbleGameUIManager.Instance.ShowAllUI(false);
        
        CKB_UI_TextDialogue.Instance.onStart = () => { state = State.Conversation; };
        CKB_UI_TextDialogue.Instance.AppearTextDialogue();
        CKB_UI_TextDialogue.Instance.EnqueueConversationText("좋습니다.");
        CKB_UI_TextDialogue.Instance.EnqueueConversationText("구슬이 1개 이상 남았습니다.");
        CKB_UI_TextDialogue.Instance.DisappearTextDialogue();
        CKB_UI_TextDialogue.Instance.onComplete = () => { state = State.End; };
    }

    void UpdateDie()
    {
        CKB_MarbleGameUIManager.Instance.ShowAllUI(false);
        player.Die(CKB_Player.DieType.FlyAway);
        
        DisconnectPhoton();
    }

    void UpdateEnd()
    {
        photonView.RPC("AddPlayerEndCount", RpcTarget.All);
        
        state = State.AllEnd;
    }

    [PunRPC]
    void AddPlayerEndCount()
    {
        playerEndCount++;
    }

    void DisconnectPhoton()
    {
        PhotonNetwork.Destroy(goPlayer);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.Disconnect();

        state = State.AllEnd;
    }
}
