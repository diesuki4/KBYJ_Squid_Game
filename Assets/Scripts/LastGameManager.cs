using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class LastGameManager : MonoBehaviourPun
{
    public Text resultText;
    
    // Start is called before the first frame update
    void Start()
    {
        resultText.text = "마지막까지 살아남은 인원은" + PhotonNetwork.CurrentRoom.PlayerCount + "명입니다";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
