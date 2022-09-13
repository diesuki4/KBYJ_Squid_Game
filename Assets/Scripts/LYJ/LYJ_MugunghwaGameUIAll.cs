using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LYJ_MugunghwaGameUIAll : MonoBehaviour
{
    public static LYJ_MugunghwaGameUIAll Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
}
