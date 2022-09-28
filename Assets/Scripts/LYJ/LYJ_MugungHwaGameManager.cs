using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class LYJ_MugungHwaGameManager : MonoBehaviour
{
    public Transform randomPos;
    public LYJ_YeongHeeState yeoungHeeState;
    public LYJ_EndLineTrigger endLineTrigger;
    
    // Start is called before the first frame update
    void Start()
    {
        Transform trRandom = randomPos.GetChild(PhotonNetwork.CurrentRoom.PlayerCount - 1);
        GameObject player = PhotonNetwork.Instantiate("Player", trRandom.position, trRandom.rotation);
        yeoungHeeState.player = player;
        endLineTrigger.player = player;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
