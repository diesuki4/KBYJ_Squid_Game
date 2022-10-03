using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class LYJ_MGGameManager : MonoBehaviourPun
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
    }

    public State state;
    
    // Start is called before the first frame update
    void Start()
    {
        Transform trRandom = randomPos.GetChild(PhotonNetwork.CurrentRoom.PlayerCount - 1);
        GameObject player = PhotonNetwork.Instantiate("PlayerMG", trRandom.position, trRandom.rotation);
        endLineTrigger.player = player;
        pmDetect = player.GetComponent<LYJ_PlayerMoveDetect>();
        attackExplosion = player.GetComponent<LYJ_AttackExplosion>();
        LYJ_MGGameUIManager.Instance.crosshair = player.transform.Find("Crosshair").gameObject;
        
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
            /*
            case State.End:
                UpdateEnd();
                break;*/
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
        CKB_UI_TextDialogue.Instance.onComplete = () => { state = State.Initialize; };
    }

    private void UpdateInitialize()
    {
        LYJ_MGGameUIManager.Instance.ShowAllUITres(false);
        LYJ_MGGameUIManager.Instance.ShowCountDownText(true);
        LYJ_MGGameUIManager.Instance.ShowMugunghwa(true);
        mugunghwaTime = Random.Range(4, 7);
        
        state = State.CanMove;
    }
  
    private void UpdateCanMove()
    {
        if (currentTime >= mugunghwaTime)
        {
            LYJ_MGGameUIManager.Instance.ShowAllUITres(false);
            LYJ_MGGameUIManager.Instance.ShowBloom(true);
            
            // 2 영희가 목을 한번 돌린다
            turnNeck.TurnBackNeck();
            
            currentTime = 0;
            
            state = State.Bloom;
        }
        else
        {
            currentTime += Time.deltaTime;
        }
    }

    private void UpdateBloom()
    {
        if (currentTime >= bloomTime)
        {
            currentTime = 0;
            turnNeck.TurnNeck();
            state = State.Initialize;
        }
        else
        {
            currentTime += Time.deltaTime;

            if (triggerGround.isInsideLine)
            {
                // 3 플레이어 움직임 감지
                isTargeted = pmDetect.IsPlayerMoving();
                
                if (isTargeted)
                {
                    state = State.Target;
                }
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

        state = State.End;
    }
        
    
    private void UpdateAttack()
    {
        StartCoroutine(IEDie());
                
        state = State.Die;
    }

    private void UpdateEnd()
    {
        Debug.Log("state = State.End");
    }
}