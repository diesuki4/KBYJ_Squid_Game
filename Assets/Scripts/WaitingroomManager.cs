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
        Transform randomTr = trPos.GetChild(PhotonNetwork.CurrentRoom.PlayerCount - 1);
        go = PhotonNetwork.Instantiate("Player", randomTr.position, randomTr.rotation);

        uniqueValue = Random.Range(float.MinValue, float.MaxValue);
        photonView.RPC("AddPlayerCount", RpcTarget.MasterClient, uniqueValue);
    }

    [PunRPC]
    void AddPlayerCount(float unqValue)
    {
        photonView.RPC("SetPlayerPosition", RpcTarget.All, playerCount++, unqValue);
    }

    [PunRPC]
    void SetPlayerPosition(int count, float unqValue)
    {
        if (Mathf.Approximately(uniqueValue, unqValue))
            go.transform.position = trPos.GetChild(count).position;
    }
}
