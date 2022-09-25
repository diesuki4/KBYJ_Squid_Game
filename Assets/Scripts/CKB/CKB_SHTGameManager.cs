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
        End = 128
    }
    State state;

    [Header("달고나 게임 시간")]
    public float inGameTime;

    public CKB_Player player;
    public Transform spawnPositions;

    float currentTime;

    Animator agentAnim;

    void Start()
    {
        int index = UnityEngine.Random.Range(0, spawnPositions.childCount);
        Transform tr = spawnPositions.GetChild(index);

        player = PhotonNetwork.Instantiate("Player", tr.position, tr.rotation).GetComponent<CKB_Player>();

        state = State.Idle;

        agentAnim = GameObject.Find("CKB/Agent").GetComponent<Animator>();
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
        CKB_SHTGameUIManager.Instance.ShowAllDrawArea(false);

        state = State.InGame;
    }

    void UpdateInGame()
    {
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
                    state = State.Result;
            }
        }
        else
        {
            state = State.Result;
        }
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
        if (CKB_GameManager.Instance.debugMode)
            Debug.Log("[CKB_SHTGameManager] 살아남았습니다!!");

        state = State.End;
    }

    void UpdateDie()
    {
        agentAnim.SetTrigger("Load");

        player.Die(CKB_Player.DieType.FlyAway);
        state = State.End;

        if (CKB_GameManager.Instance.debugMode)
            Debug.Log("[CKB_SHTGameManager] 죽었습니다!!");

        state = State.End;
    }
}
