using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class LYJ_MGGameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    #region gameobject
    public LYJ_MGTurnNeck turnNeck;
    private LYJ_PlayerMoveDetect pmDetect;
    private LYJ_AttackExplosion attackExplosion;
    public LYJ_TriggerGround triggerGround;
    public LYJ_EndLineTrigger endLineTrigger;
    #endregion
    
    #region player
    public Transform randomPos;
    #endregion
    
    #region randomTime
    private float currentTime;
    public float mugunghwaTime;
    private float bloomTime = 3;
    #endregion

    #region movingDetect
    private LYJ_PlayerMoveDetect _playerMoveDetect;
    private bool targetForAttack;
    #endregion

    #region playerDead
    private float power = 10.0f;
    private float radius = 5.0f;
    private float upForce = 0.1f;
    #endregion

    public bool isTargeted;
    private bool isOnce;
    public GameObject cube;
    
    public float timeValue = 150;
    private bool isGameStarted;

    private float uniqueValues;
    private int playerCount;
    private GameObject player;
    private int playerEndCount;

    bool isEnd;
    bool isTimeOut;
    bool isTimeOutChecked;

    /* 상태머신 */
    public enum State
    {
        Idle = 1,
        Conversation = 2,
        Initialize = 4,
        CanMove = 8,
        Bloom = 16,
        Target = 32,
        Attack = 64,
        Die = 128,
        End = 256,
        AllEnd = 512,
    }

    public State state;
    
    // Start is called before the first frame update
    void Start()
    {
        player = PhotonNetwork.Instantiate("PlayerMG", new Vector3(0, 2.5f, -43.4f), Quaternion.identity);
        endLineTrigger.player = player;
        triggerGround.player = player;
        pmDetect = player.GetComponent<LYJ_PlayerMoveDetect>();
        attackExplosion = player.GetComponent<LYJ_AttackExplosion>();
        LYJ_MGGameUIManager.Instance.crosshair = player.transform.Find("Crosshair").gameObject;

        uniqueValues = Random.Range(float.MinValue, float.MaxValue);
        
        photonView.RPC("RequestSetPos", RpcTarget.MasterClient, uniqueValues);
        
        state = State.Idle;
    }

    [PunRPC]
    private void RequestSetPos(float unqValue)
    {
        photonView.RPC("RpcSetPlayerPosition", RpcTarget.All, playerCount, unqValue);
        photonView.RPC("SetPlayerCount", RpcTarget.Others, ++playerCount);  // 방장 나갔을 때 대비해서 playerCount 초기화
        
    }

    [PunRPC]
    private void RpcSetPlayerPosition(int posIdx, float unqValue)
    {
        if (Mathf.Approximately(this.uniqueValues, unqValue))
        {
            player.transform.position = randomPos.GetChild(posIdx).position;
            player.transform.rotation = randomPos.GetChild(posIdx).rotation;
        }
    }

    [PunRPC]
    private void SetPlayerCount(int playerCount)
    {
        this.playerCount = playerCount;
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
            case State.Initialize:
                UpdateInitialize();
                break;
            case State.CanMove:
                UpdateCanMove();
                break;
            case State.Bloom:
                UpdateBloom();
                break;
            case State.Target:
                UpdateTarget();
                break;
            case State.Attack:
                UpdateAttack();
                break;
            case State.Die:
                break;
            case State.End:
                UpdateEnd();
                break;
            case State.AllEnd:
                break;
        }

        if (isGameStarted)
        {
            Timer();
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
                    PhotonNetwork.LoadLevel("BridgeScene");
                }

                isEnd = true;
            }
        }
    }

    private void Timer()
    {
        if (PhotonNetwork.IsMasterClient)
            if (timeValue > 0)
                timeValue -= Time.deltaTime;

        if (timeValue <= 0)
            isTimeOut = true;

        LYJ_MGGameUIManager.Instance.SetCountDownText(timeValue);
        LYJ_MGGameUIManager.Instance.ShowCountDownText(true);

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            stream.SendNext(timeValue);
            stream.SendNext(currentTime);
            stream.SendNext(mugunghwaTime);
        }
        else
        {
            timeValue = (float)stream.ReceiveNext();
            currentTime = (float)stream.ReceiveNext();
            mugunghwaTime = (float)stream.ReceiveNext();
        }
    }

    private void UpdateIdle()
    {
        /* 대기화면 */
        // 게임 설명 (UI)
        LYJ_MGGameUIManager.Instance.ShowAllUI(false);
        
        CKB_UI_TextDialogue.Instance.onStart = () => { state = State.Conversation; };
        CKB_UI_TextDialogue.Instance.AppearTextDialogue();
        CKB_UI_TextDialogue.Instance.EnqueueConversationText("이번 게임은 무궁화꽃이 피었다입니다.");
        CKB_UI_TextDialogue.Instance.EnqueueConversationText("제한 시간 내에 선 안으로 들어가면 통과입니다.");
        CKB_UI_TextDialogue.Instance.DisappearTextDialogue();
        CKB_UI_TextDialogue.Instance.onComplete = () =>
        {
            cube.SetActive(false);
            isGameStarted = true;
            state = State.Initialize;
        };
    }

    private void UpdateInitialize()
    {
        LYJ_MGGameUIManager.Instance.ShowAllUITres(false);
        LYJ_MGGameUIManager.Instance.ShowCountDownText(true);
        LYJ_MGGameUIManager.Instance.ShowMugunghwa(true);

        if (PhotonNetwork.IsMasterClient)
            mugunghwaTime = UnityEngine.Random.Range(4, 7);
        
        state = State.CanMove;
    }

    private void UpdateCanMove()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (currentTime >= mugunghwaTime)
            {
                RpcCanMoveToBloom();
                photonView.RPC("RpcCanMoveToBloom", RpcTarget.Others);
            }
            else
            {
                currentTime += Time.deltaTime;
            }
        }
    }

    [PunRPC]
    private void RpcCanMoveToBloom()
    {
        if ((state & (State.Target | State.Attack | State.Die | State.End | State.AllEnd)) == 0)
        {
            LYJ_MGGameUIManager.Instance.ShowAllUITres(false);
            LYJ_MGGameUIManager.Instance.ShowBloom(true);
                    
            // 2 영희가 목을 한번 돌린다
            turnNeck.TurnBackNeck();
                    
            currentTime = 0;
                    
            state = State.Bloom;
        }
    }

    private void UpdateBloom()
    {
        if (triggerGround.isInsideLine)
        {
            // 3 플레이어 움직임 감지
            isTargeted = pmDetect.IsPlayerMoving();
            
            if (isTargeted)
            {
                if (PhotonNetwork.IsMasterClient)
                    MakingMasterClient();
                    
                state = State.Target;
            }
        }
        if (PhotonNetwork.IsMasterClient)
        {
            if (currentTime >= bloomTime)
            {
                BloomToInitialize();
                photonView.RPC("BloomToInitialize", RpcTarget.Others);
            }
            else
            {
                currentTime += Time.deltaTime;
            }
        }
    }

    [PunRPC]
    private void BloomToInitialize()
    {
        if (state == State.Bloom)
        {
            currentTime = 0;
            turnNeck.TurnNeck();
            state = State.Initialize;
        }
    }

    private void MakingMasterClient()
    {
        if (PhotonNetwork.PlayerListOthers.Length > 0)
        {
            int idx = UnityEngine.Random.Range(0, PhotonNetwork.PlayerListOthers.Length);
            PhotonNetwork.SetMasterClient(PhotonNetwork.PlayerListOthers[idx]);
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);

        if (PhotonNetwork.IsMasterClient)
        {
            if (state != State.Bloom)
            {
                MakingMasterClient();
            }
        }
    }

    private void UpdateTarget()
    {
        // 4 타깃 크로스헤어 띄우고
        LYJ_MGGameUIManager.Instance.ShowCrosshair(true);

        state = State.Attack;
    }

    private IEnumerator IEDie()
    {
        yield return new WaitForSeconds(1);
        attackExplosion.AttackExplosion(pmDetect.lastPos, power, radius, upForce);

        DisconnectPhoton();
    }
    
    private void UpdateAttack()
    {
        StartCoroutine(IEDie());
                
        state = State.Die;
    }

    private void UpdateEnd()
    {
        photonView.RPC("AddPlayerEndCount", RpcTarget.All);

        state = State.AllEnd;
    }

    [PunRPC]
    private void AddPlayerEndCount()
    {
        playerEndCount++;
    }

    void DisconnectPhoton()
    {
        PhotonNetwork.Destroy(player);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.Disconnect();

        state = State.AllEnd;
    }
}