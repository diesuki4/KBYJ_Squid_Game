using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LYJ_TimerLobby : MonoBehaviour
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
            timeValue -= Time.deltaTime; // 프레임 == 60프레임에 1초 => 
            min = (int)timeValue / 60;
            sec = (int)timeValue % 60;

        }
        else
        {
            timeValue += 100;
            // State.End로 변환
            LYJ_YeongHeeState lyjState = LYJ_YeongHee.Instance.GetComponent<LYJ_YeongHeeState>();
            lyjState.state = LYJ_YeongHeeState.State.End;
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
        print("min: " + min + "sec: " + sec);

        // Debug.Log((int)timeValue);
        ledBoard.LedText = string.Format(" 0{0}", min);

        if (sec < 10)
        {
            ledBoard.LedText += string.Format(" : 0{0} ", sec);
        }
        else
        {
            ledBoard.LedText += string.Format(" : {00} ", sec);
        }
        // ledBoard.LedText = string.Format(" {0:D2} : {0:D2} ", min, sec);
    }
}
