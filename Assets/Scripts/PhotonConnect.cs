using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonConnect : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    void Update() { }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        PhotonNetwork.NickName = "플레이어" + Random.Range(0, 1000).ToString();
        
        PhotonNetwork.JoinLobby();
        
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        
        CreateRoom();
    }
    
    
    /* 방생성 */
    private void CreateRoom()
    {
        // 1 방 옵션 설정
        RoomOptions roomOptions = new RoomOptions();
        
        // 1-1 최대인원(0이면 최대인원 = 256)
        // Return byte; => byte n = 10; => 1111 1111 = 256
        roomOptions.MaxPlayers = 20;
        
        // 1-2 룸 리스트에 보이지 않게 || 보이게? 
        roomOptions.IsVisible = true;   // 공개/비밀방 여부(default: true)
        
        // 2 방 생성 '요청'(방 옵션을 이용해서) 
        PhotonNetwork.CreateRoom("SquidRoom", roomOptions);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        
        JoinRoom();
    }

    /* 방 참가 */
    public void JoinRoom()
    {
        // 1 방 참가 '요청'
        // PhotonNetwork.JoinRoom("XR_B반");
        PhotonNetwork.JoinRoom("SquidRoom");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        PhotonNetwork.LoadLevel("BridgeScene");
    }
    
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("OnJoinRoomFailed , " + returnCode + ", " + message);
    }
}