using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LYJ_LedBoard : MonoBehaviour
{
    public static LYJ_LedBoard Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
}
