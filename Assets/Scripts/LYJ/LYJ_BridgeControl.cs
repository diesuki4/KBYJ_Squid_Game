using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class LYJ_BridgeControl : MonoBehaviourPun
{
    public bool[] usingGravityS = new bool[22];
    GameObject[] scaffoldingS = new GameObject[22];
    
    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity = new Vector3(0, -70, 0);

        if (PhotonNetwork.IsMasterClient)
        {
            CreateRandomValue();
            photonView.RPC("SetUsingGravityArray", RpcTarget.All, usingGravityS);
        }

        BringGameObject();

        SetBridgeGravity();
    }

    [PunRPC]
    private void SetUsingGravityArray(bool[] usingGravityS)
    {
        this.usingGravityS = usingGravityS;
    }
    
    private void CreateRandomValue()
    {
        Debug.Log("usingGravityS.Length: " + usingGravityS.Length);
        
        for (int i = 0; i < usingGravityS.Length; i++)
        {
            if (i % 2 == 0)
            {
                // print("i 짝수: " + i);
                // 0 - false / 1 - true
                int result = UnityEngine.Random.Range(0, 2);
                
                // result = 0
                Debug.Log("result: " + result);
                bool temp = false;

                switch (result)
                {
                    case 0:
                        temp = false;
                        break;
                    case 1:
                        temp = true;
                        break;
                }
                Debug.Log("temp: " + temp);
                
                usingGravityS[i] = temp;
            }

            if (i % 2 != 0)
            {
                int y = i - 1;
                // print("i: 홀수" + i);

                switch (usingGravityS[y])
                {
                    case false:
                        usingGravityS[i] = true;
                        break;
                    case true:
                        usingGravityS[i] = false;
                        break;
                }
            }
            // Debug.Log("usingGravityS[" + i + "]: "  + usingGravityS[i]);
        }
    }

    private void BringGameObject()
    {
        for (int i = 1; i <= scaffoldingS.Length; i++)
        {
            int y = i - 1;
            // [] 0 ~ 21 / game object: 1 ~ 22
            scaffoldingS[y] = GameObject.Find("Scaffolding (" + i + ")");
            // Debug.Log("scaffoldingS[" + y + "]: " + scaffoldingS[y]);
        }
    }
    
    [PunRPC]
    private void SetBridgeGravity()
    {
        for (int i = 0; i < scaffoldingS.Length; i++)
        {
            scaffoldingS[i].GetComponent<LYJ_Bridge>().usingGravity = usingGravityS[i];
            // Debug.Log(scaffoldingS[i].GetComponent<LYJ_Bridge>());
        }
    }
}
