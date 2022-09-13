using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LYJ_YeongHee : MonoBehaviour
{
    public static LYJ_YeongHee Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
}
