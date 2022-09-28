using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class WaitingroomManager : MonoBehaviour
{
    public Transform trPos;
    
    // Start is called before the first frame update
    void Start()
    {
        Transform randomTr = trPos.GetChild(PhotonNetwork.CurrentRoom.PlayerCount - 1);
        PhotonNetwork.Instantiate("Player", randomTr.position, randomTr.rotation);
    }
}
