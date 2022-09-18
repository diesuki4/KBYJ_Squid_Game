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
        nickname.GetComponent<Text>().text = photonView.Owner.NickName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
