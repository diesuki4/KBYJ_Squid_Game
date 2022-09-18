using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private bool explosion;
    public GameObject player; 

    // Start is called before the first frame update
    void Start()
    {
        attackExplosion = GetComponent<LYJ_AttackExplosion>();
        _playerMoveDetect = player.GetComponent<LYJ_PlayerMoveDetect>();
        
        print("attackExplosion: " + attackExplosion + " _playerMoveDetect: " + _playerMoveDetect);
    }

    // Update is called once per frame
    void Update()
    {
        if (explosion)
        {
            explosion = false;
            attackExplosion.AttackExplosion(_playerMoveDetect.lastPos, power, radius, upForce);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Player"))
        {
            explosion = true;
        }
    }
}
