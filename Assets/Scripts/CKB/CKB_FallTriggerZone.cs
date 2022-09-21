using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKB_FallTriggerZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Player"))
            other.GetComponent<CKB_Player>().state = CKB_Player.State.Die;
    }
}
