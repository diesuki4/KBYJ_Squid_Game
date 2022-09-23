using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class LYJ_MugungHwaGameManager : MonoBehaviour
{
    public Transform randomPos;
    public GameObject yeoungHee;
    
    // Start is called before the first frame update
    void Start()
    {
        Transform trRandom = randomPos.GetChild(Random.Range(0, randomPos.childCount));
        GameObject player = PhotonNetwork.Instantiate("Player", trRandom.position, trRandom.rotation);
        yeoungHee.GetComponent<LYJ_YeongHeeState>().player = player;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
