using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LYJ_Temp : MonoBehaviour
{
    private CharacterController cc;
    
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        cc.Move(Vector3.positiveInfinity);
    }
}
