using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKB_GameManager : MonoBehaviour
{
    public static CKB_GameManager Instance;

    [Header("디버그 모드")]
    public bool debugMode;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    void Start() { }

    void Update()
    {
        if (debugMode)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Time.timeScale = 1;
                Debug.Log("[CKB_GameManager] timeScale = 1");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Time.timeScale = 2;
                Debug.Log("[CKB_GameManager] timeScale = 2");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Time.timeScale = 3;
                Debug.Log("[CKB_GameManager] timeScale = 3");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                Time.timeScale = 4;
                Debug.Log("[CKB_GameManager] timeScale = 4");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                Time.timeScale = 5;
                Debug.Log("[CKB_GameManager] timeScale = 5");
            }

        }
    }
}
