using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class LYJ_BridgeGameManager : MonoBehaviourPunCallbacks
{
    private GameObject player;
    public Transform randomPos;
    public GameObject ground;
    public LYJ_BREndLineTrigger endLineTrigger;
    public GameObject hiddenBridge;

    public int endPlayerCount;
    private bool isEnd;

    public TextMeshProUGUI countDownText;
    public float timeValue = 150;
    private bool isTimeOut;

    private void Awake()
    {
        //0~19
        /*Transform[] randomPosS = randomPos.GetComponentsInChildren<Transform>();
        int randomNum = UnityEngine.Random.Range(0, maxPlayer);
        Transform randomTr = randomPosS[randomNum];*/
        Transform randomTr = randomPos.GetChild(PhotonNetwork.CurrentRoom.PlayerCount - 1);
        player = PhotonNetwork.Instantiate("Player", randomTr.position, randomTr.rotation);
        ground.GetComponent<LYJ_BridgeDie>().player = player;
        endLineTrigger.player = player;

    }

    // Start is called before the first frame update
    void Start()
    {
        hiddenBridge.SetActive(false);
    }
    
    // Update is called once per frame
    void Update()
    {
        SetHiddenBridge(true);
        
        print("endPlayerCount: " + endPlayerCount);
        if (PhotonNetwork.IsMasterClient)
        {
            if (endPlayerCount == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                if (player.GetComponent<CKB_Player>().state == CKB_Player.State.Die && isEnd == false)
                {
                    isEnd = true;
                    PhotonNetwork.LeaveRoom();
                    PhotonNetwork.LeaveLobby();
                    PhotonNetwork.Disconnect();
                    Application.Quit();
                }
                if (player.GetComponent<CKB_Player>().state != CKB_Player.State.Die && isEnd == false)
                {
                    photonView.RPC("RpcLoadScene", RpcTarget.All);
                    isEnd = true;
                }
            }
        }
        Timer();
    }

    private void Timer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (timeValue > 0)
                timeValue -= Time.deltaTime;

            if (timeValue <= 0)
                isTimeOut = true;
            
            ShowCountDown(timeValue, true);
        }
    }

    private void ShowCountDown(float countdown, bool show)
    {
        countDownText.text = Mathf.CeilToInt(countdown).ToString();
        countDownText.gameObject.SetActive(show);
    }

    public void CountUp()
    {
        photonView.RPC("RpcCountUp", RpcTarget.MasterClient);
    }

    [PunRPC]
    private void RpcCountUp()
    {
        endPlayerCount++;
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
            PhotonNetwork.LoadLevel("CKB_SHTGameScene");
            isEnd = true;
        }
    }

    private void SetHiddenBridge(bool show)
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            hiddenBridge.SetActive(show);
        }
    }
}
