using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PhotonPlayer : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {
        Transform nickname = transform.Find("Canvas/Nickname");

        if (CKB_GameManager.Instance.photonMode)
            nickname.GetComponent<Text>().text = photonView.Owner.NickName;
        else
            nickname.GetComponent<Text>().text = "플레이어";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
