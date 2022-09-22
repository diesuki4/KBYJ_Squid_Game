using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPun
{
    public Text roomNameUI;
    public Text playerNum;
    public Button startButton;
    
    // Start is called before the first frame update
    void Start()
    {
        print(PhotonNetwork.CurrentRoom.Name);
        roomNameUI.text = "방이름: " + PhotonNetwork.CurrentRoom.Name;
        playerNum.text = "참가자수: " + PhotonNetwork.CurrentRoom.PlayerCount;

        if (PhotonNetwork.IsMasterClient)
            startButton.interactable = true;
        else
            startButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    // 방장이 master
    public void OnClickStartButton()
    {
        if (PhotonNetwork.IsMasterClient)
            photonView.RPC("RpcLoadLevel", RpcTarget.All);
    }

    [PunRPC]
    private void RpcLoadLevel()
    {
        PhotonNetwork.LoadLevel("WaitingroomScene");
    }
}
