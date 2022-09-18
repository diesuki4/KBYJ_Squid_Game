using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKB_DieTriggerZone : MonoBehaviour
{
    CKB_Player player;

    void Start()
    {
        player = GetComponent<CKB_Player>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Player"))
        {
            player.Die(CKB_Player.DieType.Disassemble);
        }
    }
}
