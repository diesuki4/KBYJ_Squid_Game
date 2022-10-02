using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CKB_SHTGameManager : MonoBehaviourPun
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
        Alive = 32,
        Die = 64,
        End = 128,
        AllEnd = 256,
    }
    State state;

    [Header("달고나 게임 시간")]
    public float inGameTime;

    GameObject goPlayer;
    public CKB_Player player;
    public Transform spawnPositions;
    public Transform spawnPositionsAgent;

    float currentTime;

    Animator agentAnim;

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
            goPlayer.transform.position = spawnPositions.GetChild(posIdx).position;
            goPlayer.transform.rotation = spawnPositions.GetChild(posIdx).rotation;

            agentAnim.transform.position = spawnPositionsAgent.GetChild(posIdx).position;
            agentAnim.transform.rotation = spawnPositionsAgent.GetChild(posIdx).rotation;
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
            case State.InGame :
                UpdateInGame();
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

        EndDetect();
    }

    void EndDetect()
    {
        if (PhotonNetwork.IsMasterClient && isEnd == false)
        {
            if (playerEndCount == playerCount)
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
                    PhotonNetwork.LoadLevel("CKB_MarbleGameScene");
                }

                isEnd = true;
            }
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
        CKB_SHTGameUIManager.Instance.ShowAllDrawArea(false);

        state = State.InGame;
    }

    bool isInGameEnd;

    void UpdateInGame()
    {
        if (isInGameEnd)
            return;

        currentTime += Time.deltaTime;

        if (currentTime < inGameTime)
        {
            CKB_SHTGameUIManager.Instance.SetCountDownText(inGameTime - currentTime);

            if (Input.GetMouseButton(0))
            {
                if (Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0)
                    CKB_SHTGameUIManager.Instance.DrawCircle(Input.mousePosition);
                    
                Image hitImage = CKB_SHTGameUIManager.Instance.GraphicRaycast(Input.mousePosition);

                if (hitImage)
                    if (CKB_SHTGameUIManager.Instance.IsInnerArea(hitImage))
                        CKB_SHTGameUIManager.Instance.ShowDrawArea(hitImage, true);
                    else
                        state = State.Result;

                if (CKB_SHTGameUIManager.Instance.IsAllDrawAreaVisible())
                {
                    StartCoroutine(IESetState(State.Result, 0.5f));
                    isInGameEnd = true;
                }
            }
        }
        else
        {
            state = State.Result;
        }
    }

    IEnumerator IESetState(State state, float delay)
    {
        yield return new WaitForSeconds(delay);
        this.state = state;
    }

    void UpdateResult()
    {
        bool result = CKB_SHTGameUIManager.Instance.IsAllDrawAreaVisible();

        CKB_SHTGameUIManager.Instance.ShowAllUI(false);

        CKB_UI_TextDialogue.Instance.onStart = () => { state = State.Conversation; };
        CKB_UI_TextDialogue.Instance.AppearTextDialogue();
        CKB_UI_TextDialogue.Instance.EnqueueConversationText("게임이 종료되었습니다.");
        CKB_UI_TextDialogue.Instance.EnqueueConversationText("달고나가 잘 뽑" + (result ? "혔습니다. " : "히지 않았습니다."));
        if (!result) CKB_UI_TextDialogue.Instance.EnqueueConversationText("아쉽군요.");
        CKB_UI_TextDialogue.Instance.DisappearTextDialogue();
        CKB_UI_TextDialogue.Instance.onComplete = () => { state = (result) ? State.Alive : State.Die; };
    }

    void UpdateAlive()
    {
        state = State.End;
    }

    void UpdateDie()
    {
        agentAnim.SetTrigger("Load");

        player.Die(CKB_Player.DieType.FlyAway);
        state = State.End;
    }

    void UpdateEnd()
    {
        // PlayerEndCount 증가
        AddEndCount();
        
        state = State.AllEnd;
    }

    public void AddEndCount()
    {
        photonView.RPC("RpcAddEndCount", RpcTarget.All);
    }

    [PunRPC]
    void RpcAddEndCount()
    {
        ++playerEndCount;
    }
}
