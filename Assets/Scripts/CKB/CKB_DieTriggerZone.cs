using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKB_DieTriggerZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Player"))
        {
            CKB_Player.Instance.Die(CKB_Player.DieType.Disassemble);
        }
    }
}
