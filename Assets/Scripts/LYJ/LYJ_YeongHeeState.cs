using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class LYJ_YeongHeeState : MonoBehaviourPunCallbacks, IPunObservable
{
    #region randomTime
    private float currentTime;
    private float mugunghwaTime;
    private float rayTime;
    private float bloomTime = 3;
    #endregion

    #region UI
    public GameObject canvasMugunghwa;
    public GameObject canvasBloom;
    public GameObject crosshair;
    public GameObject canvasTimer;
    #endregion

    #region movingDetect
    private LYJ_PlayerMoveDetect _playerMoveDetect;
    private bool targetForAttack;
    #endregion

    #region attack
    public GameObject yeongHeeHead;
    #endregion

    #region playerDead
    public float power = 10.0f;
    public float radius = 5.0f;
    public float upForce = 0.1f;
    #endregion

    public GameObject player;

    public int playerEndCount;
    private bool isEnd;
    public bool isTargeted;

    private bool isRandomValueCreated;
    private bool isCanvasTimerActive;
    private bool isCanvasMugunghwaActive;
    private bool isCanvasBloomActive;
    private bool isCrosshairActive;

    #region relatedYeongHee
    private float headTime = 2;
    private float curTime;
    #endregion

    /* 상태머신 */
    public enum State
    {
        Idle = 1,
        Conversation = 2,
        Initialize = 4,
        TurnYhNeck = 8,
        UIInitialize = 32,
        Bloom = 64,
        Detect = 128,
        Target = 256,
        Attack = 512,
        Die = 1024,
        End = 2048,
    }

    public State state;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerMoveDetect = player.GetComponent<LYJ_PlayerMoveDetect>();

        state = State.Idle;
        
        canvasMugunghwa.SetActive(false);
        canvasTimer.SetActive(false);
        canvasBloom.SetActive(false);
        crosshair.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("targetForAttack: " + targetForAttack);
        // Debug.Log("playerEndCount: " + playerEndCount);
        

        if (PhotonNetwork.IsMasterClient)
        {
            if (playerEndCount == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                // 게임 종료
                state = State.End;
            }
        }
        
        canvasTimer.SetActive(isCanvasTimerActive);
        canvasMugunghwa.SetActive(isCanvasMugunghwaActive);
        canvasBloom.SetActive(isCanvasBloomActive);
        crosshair.SetActive(isCrosshairActive);
        
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
            case State.TurnYhNeck:
                UpdateTurnYhNeck();
                break;
            case State.UIInitialize:
                UpdateUIInitialize(state);
                break;
            case State.Bloom:
                UpdateBloom();
                break;/*
            case State.Detect:
                UpdateDetect();
                break;
            case State.Target:
                UpdateTarget();
                break;
            case State.Attack:
                UpdateAttack();
                break;
            case State.Die:
                // UpdateDie();
                break;
            case State.End:
                UpdateEnd();
                break;*/
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
        CKB_UI_TextDialogue.Instance.EnqueueConversationText("이번 게임은 무궁화꽃이 피었다입니다.");
        CKB_UI_TextDialogue.Instance.EnqueueConversationText("제한 시간 내에 선 안으로 들어가면 통과입니다.");
        CKB_UI_TextDialogue.Instance.DisappearTextDialogue();
        CKB_UI_TextDialogue.Instance.onComplete = () =>
        {
            if (PhotonNetwork.IsMasterClient)
            {
                // photonView.RPC("RpcChangeState", RpcTarget.All, State.Initialize);
                state = State.Initialize;
            }
        };
    }

    private void UpdateInitialize()
    {
        mugunghwaTime = Random.Range(4, 7);
        rayTime = mugunghwaTime - 1;

        state = State.TurnYhNeck;
    }

    private void UpdateUIInitialize(State state)
    {
        isCanvasMugunghwaActive = false;
        targetForAttack = false;
        isCrosshairActive = false;

        this.state = state;
    }

    private void UpdateTurnYhNeck()
    {
        /* UI 표시 */
        // if (PhotonNetwork.IsMasterClient)
        // {
        isCanvasTimerActive = true;
        currentTime += Time.deltaTime;
            
        // 2 mugunghwaTime 이 될 때까지
        if (currentTime < mugunghwaTime)
        {
            // 3 관련 UI를 표시한다
            isCanvasMugunghwaActive = true;
            
            // 함수 실행
            /* 영희 머리 돌아가기 */
            if (currentTime <= 1)
                yeongHeeHead.transform.eulerAngles = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(0, 180, 0), currentTime);        
        }
        else
        {
            currentTime = 0;
            // photonView.RPC("RpcChangeState", RpcTarget.All, State.Bloom);
            UpdateUIInitialize(State.Bloom);
        }
        // }
        // 1 만약 타깃이 되면
        /*if (targetForAttack == true)
        {
            state = State.Attack;
        }*/
    }

    private void UpdateBloom()
    {
        // Debug.Log("state = State.Bloom");
        
        /* 피었습니다 */
        // 1 시간이 흐르고

        // if (PhotonNetwork.IsMasterClient)
        // {
        currentTime += Time.deltaTime;
        // 2 bloomTime 이 될 때까지
        if (currentTime < bloomTime)
        {
            // print(currentTime);
            // 3 관련 UI를 표시한다
            isCanvasBloomActive = true;

            if (currentTime <= 1)
                yeongHeeHead.transform.eulerAngles = Vector3.Lerp(new Vector3(0, 180, 0), new Vector3(0, 0, 0), currentTime);
        }
        else
        {
            currentTime = 0;
            isCanvasBloomActive = false;
            state = State.TurnYhNeck;
        }
        // }
        state = State.Detect;
    }

    private void UpdateTarget()
    {
        // 2 타깃 위에 crosshair 표시하고
        crosshair.SetActive(true);
        crosshair.transform.position = new Vector3(_playerMoveDetect.lastPos.x + 0.7f, _playerMoveDetect.lastPos.y + 2.5f, _playerMoveDetect.lastPos.z);
        crosshair.transform.eulerAngles = _playerMoveDetect.lastRot;

        isTargeted = true;

        state = State.Attack;
    }

    private void UpdateAttack()
    {
        // 3 끝나기 1초전에 쏜다 
        if (currentTime >= rayTime && targetForAttack)
        {
            player.GetComponent<LYJ_AttackExplosion>().AttackExplosion(_playerMoveDetect.lastPos, power, radius, upForce);
            targetForAttack = false;
            
            if (PhotonNetwork.IsMasterClient == false)
            {
                CountUp();
                state = State.End;
            }
            else
            {
                playerEndCount++;
            }
        }
    }


    private void UpdateDetect()
    {
        /* 움직임 감지 */
        if (_playerMoveDetect.isMoving && player.GetComponent<LYJ_MGPlayerInsideline>().insideLine)
        {
            targetForAttack = true;
        }
    }

    private void UpdateEnd()
    {
        Debug.Log("state = State.End");
        // 다음 씬 로드
        if (PhotonNetwork.IsMasterClient && isEnd == false)
        {
            if (player.GetComponent<CKB_Player>().state == CKB_Player.State.Die)
            {
                isEnd = true;
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.LeaveLobby();
                PhotonNetwork.Disconnect();
                Application.Quit();
            }
            else
            {
                photonView.RPC("RpcLoadScene", RpcTarget.All);
            }
        }
    }

    public void CountUp()
    {
        photonView.RPC("RpcCountUp", RpcTarget.MasterClient);
    }
    
    [PunRPC]
    private void RpcCountUp()
    {
        playerEndCount++;
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);

        RpcLoadScene();
    }

    [PunRPC]
    private void RpcLoadScene()
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
            isEnd = true;
        }
    }

    [PunRPC]
    private void RpcChangeState(State state)
    {
        this.state = state;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            stream.SendNext(isCanvasTimerActive);
            stream.SendNext(isCanvasMugunghwaActive);
            
        }
        else
        {
            isCanvasTimerActive = (bool)stream.ReceiveNext();
            isCanvasMugunghwaActive = (bool)stream.ReceiveNext();
        }
    }
}
