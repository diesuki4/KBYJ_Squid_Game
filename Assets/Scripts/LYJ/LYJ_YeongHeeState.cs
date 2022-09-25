using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class LYJ_YeongHeeState : MonoBehaviourPun
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
    private LYJ_YeongHee yeongHeeHead;
    #endregion

    #region playerDead
    public float power = 10.0f;
    public float radius = 5.0f;
    public float upForce = 0.1f;
    #endregion

    public GameObject player;

    private int playerEndCount;
    private bool isEnd;
    
    /* 상태머신 */
    public enum State
    {
        Idle,
        Conversation,
        Mugunghwa,
        Bloom,
        Die,
        End,
    }

    public State state;
    
    // Start is called before the first frame update
    void Start()
    {
        yeongHeeHead = GetComponentInChildren<LYJ_YeongHee>();
        _playerMoveDetect = player.GetComponent<LYJ_PlayerMoveDetect>();

        state = State.Mugunghwa;
        // state = State.Idle;
        
        canvasMugunghwa.SetActive(false);
        canvasTimer.SetActive(false);
        canvasBloom.SetActive(false);
        crosshair.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("targetForAttack: " + targetForAttack);
        Debug.Log("playerEndCount: " + playerEndCount);
        

        if (PhotonNetwork.IsMasterClient)
        {
            if (playerEndCount == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                // 게임 종료
                state = State.End;
            }
        }
        
        switch (state)
        {
            case State.Idle:
                UpdateIdle();
                break;
            case State.Conversation:
                break;
            case State.Mugunghwa:
                UpdateMugunghwa();
                break;
            case State.Bloom:
                UpdateBloom();
                break;
            case State.Die:
                // UpdateDie();
                break;
            case State.End:
                UpdateEnd();
                break;
        }
    }

    [PunRPC]
    void RPCSetRandomValue(int mugunghwaTime)
    {
        this.mugunghwaTime = mugunghwaTime;
        rayTime = mugunghwaTime - 1;
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
        CKB_UI_TextDialogue.Instance.onComplete = () => { state = State.Mugunghwa; };
    }
    
    private void UpdateMugunghwa()
    {
        
        if (PhotonNetwork.IsMasterClient)
        {
            // photonView.RPC("CreatingRandomValue", RpcTarget.MasterClient);
            photonView.RPC("RPCSetRandomValue", RpcTarget.All, Random.Range(2, 7));
        }
        
        // Debug.Log("state = State.Mugunghwa");
        canvasTimer.SetActive(true);
        
        /* UI 표시 */
        // 1 시간이 흐르고
        currentTime += Time.deltaTime;
        // 2 mugunghwaTime 이 될 때까지
        if (currentTime < mugunghwaTime)
        {
            // 3 관련 UI를 표시한다
            canvasMugunghwa.SetActive(true);

            if (currentTime <= 1)
            {
                /* 영희 머리 돌아가기 */
                yeongHeeHead.transform.eulerAngles = Vector3.Lerp(new Vector3(0, 180, 0), new Vector3(0, 0, 0),  currentTime);
            }
        }
        else
        {
            currentTime = 0;
            
            canvasMugunghwa.SetActive(false);
            targetForAttack = false;
            crosshair.SetActive(false);
            state = State.Bloom;
        }
        
        /* 공격 */
        // 1 만약 타깃이 되면
        if (targetForAttack == true)
        {
            // 2 타깃 위에 crosshair 표시하고
            crosshair.SetActive(true);
            crosshair.transform.position = new Vector3(_playerMoveDetect.lastPos.x, _playerMoveDetect.lastPos.y + 2.5f, _playerMoveDetect.lastPos.z);
            crosshair.transform.eulerAngles = _playerMoveDetect.lastRot;
        }

        // 3 끝나기 1초전에 쏜다 
        if (currentTime >= rayTime && targetForAttack)
        {
            player.GetComponent<LYJ_AttackExplosion>().AttackExplosion(_playerMoveDetect.lastPos, power, radius, upForce);
            
            if (PhotonNetwork.IsMasterClient == false)
            {
                photonView.RPC("RpcCountUp", RpcTarget.MasterClient);
                state = State.End;
            }
            else
            {
                playerEndCount++;
            }
        }
    }

    private void UpdateBloom()
    {
        // Debug.Log("state = State.Bloom");
        
        /* 피었습니다 */
        // 1 시간이 흐르고
        currentTime += Time.deltaTime;
        // 2 bloomTime 이 될 때까지
        if (currentTime < bloomTime)
        {
            // print(currentTime);
            // 3 관련 UI를 표시한다
            canvasBloom.SetActive(true);
            
            if (currentTime <= 1)
            {
                /* 영희 머리 돌아가기 */
                yeongHeeHead.transform.eulerAngles = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(0, 180, 0),  currentTime);
            }
        }
        else
        {
            currentTime = 0;
            canvasBloom.SetActive(false);
            state = State.Mugunghwa;
        }
        
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
            photonView.RPC("RpcLoadScene", RpcTarget.All);
        }
    }

    [PunRPC]
    private void RpcCountUp()
    {
        playerEndCount++;
    }

    [PunRPC]
    private void RpcLoadScene()
    {
        PhotonNetwork.LoadLevel("CKB_SHTGameScene");
        isEnd = true;
    }
}
