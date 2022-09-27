using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class LYJ_TimerLobby : MonoBehaviourPun, IPunObservable
{    
    public float timeValue = 100;

    #region LEDTimer import
    private string LedText;
    private LedBoardScript ledBoard;
    #endregion

    #region time
    private int min;
    private int sec;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        ledBoard = GetComponent<LedBoardScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeValue > 0)
        {
            // ☆ 1 방장한테만 카운트다운이 흐르고
            if (PhotonNetwork.IsMasterClient)
            {
                timeValue -= Time.deltaTime; // 프레임 == 60프레임에 1초 => 
            }
            min = (int)timeValue / 60;
            sec = (int)timeValue % 60;
        }
        else
        {
            timeValue += 100;
            // State.End로 변환
            // ☆ 3 00:00이 되면 방장이 다른 참가자들의 RpcLevel함수를 실행시킨다
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel("Mugunghwa");
            }
        }
        
        DisplayTime(timeValue);
        ledBoard.BoardConstructor();
    }

    void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }
        // print("min: " + min + "sec: " + sec);
        // Debug.Log((int)timeValue);
        ledBoard.LedText = string.Format(" 0{0}", min);

        if (sec < 10)
        {
            ledBoard.LedText += string.Format(":0{0} ", sec);
        }
        else
        {
            ledBoard.LedText += string.Format(":{00} ", sec);
        }
        // ledBoard.LedText = string.Format(" {0:D2} : {0:D2} ", min, sec);
    }

    // ☆ 2 방장의 시간만 업데이트 
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            stream.SendNext(timeValue);
        }
        else
        {
            timeValue = (float)stream.ReceiveNext();
        }
    }
}
