using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaitingroomManager : MonoBehaviourPun, IPunObservable
{
    public Transform trPos;
    GameObject go;
    int playerCount;
    float uniqueValue;

    public float timeValue = 20;
    public TextMeshProUGUI countDownText;
    
    // Start is called before the first frame update
    void Start()
    {
        go = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);

        uniqueValue = Random.Range(float.MinValue, float.MaxValue); // 1 player -> master
        
        if (PhotonNetwork.IsMasterClient)
            AddPlayerCount(uniqueValue);
        else
            photonView.RPC("AddPlayerCount", RpcTarget.MasterClient, uniqueValue);
    }

    private void Update()
    {
        Timer();
    }
    
    private void Timer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (timeValue > 0)
            {
                timeValue -= Time.deltaTime;
                
            }
            else
            {
                // 다음 씬 넘어가기
                PhotonNetwork.LoadLevel("Mugunghwa");

                timeValue = 100;
            }
        }
        countDownText.text = Mathf.CeilToInt(timeValue).ToString();
    }


    [PunRPC]
    void AddPlayerCount(float unqValue)
    {
        // playerCount = trPos의 자식
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            stream.SendNext(timeValue);
        }
        else
        {
            timeValue = (float)stream.ReceiveNext();
        }
    }
}
