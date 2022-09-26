using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class BridgeGameManager : MonoBehaviourPunCallbacks
{
    private GameObject player;
    public Transform randomPos;
    public GameObject ground;
    public LYJ_BREndLineTrigger endLineTrigger;

    private int endPlayerCount;
    private bool isEnd;
    
    private void Awake()
    {
        //0~19
        /*Transform[] randomPosS = randomPos.GetComponentsInChildren<Transform>();
        int randomNum = UnityEngine.Random.Range(0, maxPlayer);
        Transform randomTr = randomPosS[randomNum];*/
        Transform randomTr = randomPos.GetChild(UnityEngine.Random.Range(0, randomPos.childCount));

        player = PhotonNetwork.Instantiate("Player", randomTr.position, randomTr.rotation);
    
        ground.GetComponent<LYJ_BridgeDie>().player = player;
        endLineTrigger.player = player;

    }

    // Start is called before the first frame update
    void Start()
    {

    }
    
    // Update is called once per frame
    void Update()
    {
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
                else
                {
                    photonView.RPC("RpcLoadScene", RpcTarget.All);
                }
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
}
