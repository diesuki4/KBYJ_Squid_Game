using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKB_FallTriggerZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Player"))
        {
            CKB_Player.Instance.state = CKB_Player.State.Die;
            CKB_Player.Instance.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}
