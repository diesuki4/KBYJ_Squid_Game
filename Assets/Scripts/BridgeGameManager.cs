using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class BridgeGameManager : MonoBehaviourPun
{
    private GameObject player;
    public Transform randomPos;
    public GameObject ground;
    public LYJ_BREndLineTrigger endLineTrigger;

    private int endPlayerCount;
    
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
        if (endPlayerCount == PhotonNetwork.CurrentRoom.PlayerCount)
            ;
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
}
