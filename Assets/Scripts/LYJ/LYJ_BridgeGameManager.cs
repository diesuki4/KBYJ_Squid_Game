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
    public Transform randomPos;
    public GameObject ground;
    public LYJ_BREndLineTrigger endLineTrigger;
    public GameObject hiddenBridge;

    private float uniqueValues;
    private int playerCount;
    private GameObject player;
    private int endPlayerCount;
    bool isEnd;

    public TextMeshProUGUI countDownText;
    public float timeValue = 150;
    private bool isTimeOut;

    // Start is called before the first frame update
    void Start()
    {
        player = PhotonNetwork.Instantiate("Player", Vector3.up * 100, Quaternion.identity);

        uniqueValues = UnityEngine.Random.Range(float.MinValue, float.MaxValue);
        
        if (PhotonNetwork.IsMasterClient)
            RequestSetPos(uniqueValues);
        else
            photonView.RPC("RequestSetPos", RpcTarget.MasterClient, uniqueValues);

        ground.GetComponent<LYJ_BridgeDie>().player = player;
        endLineTrigger.player = player;

        hiddenBridge.SetActive(false);
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
