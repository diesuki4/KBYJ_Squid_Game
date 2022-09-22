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
        Transform randomTr = trPos.GetChild(Random.Range(0, trPos.childCount));
        PhotonNetwork.Instantiate("Player", randomTr.position, randomTr.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
