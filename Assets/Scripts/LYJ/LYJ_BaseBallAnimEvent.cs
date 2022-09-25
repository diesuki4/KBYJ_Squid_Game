using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LYJ_BaseBallAnimEvent : MonoBehaviour
{
    public GameObject baseball;

    public void BaseBallAnimEvent()
    {
        baseball.SetActive(false);
    }
}
