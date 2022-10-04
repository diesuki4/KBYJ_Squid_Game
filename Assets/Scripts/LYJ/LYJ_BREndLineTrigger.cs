using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LYJ_BREndLineTrigger : MonoBehaviour
{
    public LYJ_BridgeGameManager brGameManager;
    public GameObject player;

    private bool isCounted;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player && isCounted == false)
        {
            brGameManager.CountUp();
            isCounted = true;
        }
    }
}
