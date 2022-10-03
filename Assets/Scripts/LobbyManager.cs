using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public Text roomNameUI;
    public Text playerNum;
    public Button startButton;
    
    // Start is called before the first frame update
    void Start()
    {
        roomNameUI.text = "Room name: " + PhotonNetwork.CurrentRoom.Name;
        playerNum.text = "number: " + PhotonNetwork.CurrentRoom.PlayerCount;

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
            PhotonNetwork.LoadLevel("WaitingroomScene");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        playerNum.text = "참가자수: " + PhotonNetwork.CurrentRoom.PlayerCount;
    }
}
