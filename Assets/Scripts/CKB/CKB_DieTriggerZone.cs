using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKB_DieTriggerZone : MonoBehaviour
{
    public CKB_Player player;

    void Start() { }

    void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Player"))
        {
            player.Die(CKB_Player.DieType.Disassemble);
        }
    }
}
