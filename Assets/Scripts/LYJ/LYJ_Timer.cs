using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/* 시간이 흐른다 */
public class LYJ_Timer : MonoBehaviourPun
{
    public float timeValue = 150;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (timeValue > 0)
        {
            timeValue -= Time.deltaTime;
        }
        else
        {
            timeValue += 150;
        }
        DisplayTime(timeValue);
    }

    void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        LYJ_MGGameUIManager.Instance.SetCountDownText(timeValue);
        LYJ_NightGameUIManager.Instance.SetCountDownText(timeValue);
    }
}
