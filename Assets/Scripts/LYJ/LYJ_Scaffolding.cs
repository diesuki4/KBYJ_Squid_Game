using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LYJ_Scaffolding : MonoBehaviour
{
    public static LYJ_Scaffolding Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
}
