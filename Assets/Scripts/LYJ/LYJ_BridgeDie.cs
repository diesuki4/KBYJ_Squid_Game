using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LYJ_BridgeDie : MonoBehaviour
{
    #region GetComponent
    private LYJ_AttackExplosion attackExplosion;
    private LYJ_PlayerMoveDetect _playerMoveDetect;
    #endregion
    
    #region playerDead
    public float power = 10.0f;
    public float radius = 5.0f;
    public float upForce = 0.1f;
    #endregion

    public GameObject player;
    public BridgeGameManager bridgeGM;

    // Start is called before the first frame update
    void Start()
    {
        attackExplosion = player.GetComponent<LYJ_AttackExplosion>();
        _playerMoveDetect = player.GetComponent<LYJ_PlayerMoveDetect>();
        
        print("attackExplosion: " + attackExplosion + " _playerMoveDetect: " + _playerMoveDetect);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Player"))
        {
            other.GetComponent<LYJ_AttackExplosion>().photonView.RPC(
                "AttackExplosion", RpcTarget.All, _playerMoveDetect.lastPos, power, radius, upForce);
            
            bridgeGM.CountUp();
        }
    }
}
