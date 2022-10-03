using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class WaitingroomManager : MonoBehaviourPun
{
    public Transform trPos;
    GameObject go;
    int playerCount;
    float uniqueValue;
    
    // Start is called before the first frame update
    void Start()
    {
        go = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);

        uniqueValue = Random.Range(float.MinValue, float.MaxValue); // 1 player -> master
        photonView.RPC("AddPlayerCount", RpcTarget.MasterClient, uniqueValue);
    }

    [PunRPC]
    void AddPlayerCount(float unqValue)
    {
        photonView.RPC("SetPlayerPosition", RpcTarget.All, playerCount, unqValue);    // 2 master -> all
        photonView.RPC("RpcSetPlayerCount", RpcTarget.Others, ++playerCount);
    }

    [PunRPC]
    void SetPlayerPosition(int count, float unqValue)
    {
        if (Mathf.Approximately(uniqueValue, unqValue)) // 3 unqValue 내가 방장한테 보낸 거 확인하는 용도
        {
            go.transform.position = trPos.GetChild(count).position;
            go.transform.rotation = trPos.GetChild(count).rotation;
        }
    }

    [PunRPC]
    void RpcSetPlayerCount(int playerCount)
    {
        this.playerCount = playerCount;
    }
}
