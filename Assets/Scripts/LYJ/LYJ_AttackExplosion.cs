using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LYJ_AttackExplosion : MonoBehaviour
{
    private bool noUp = true;
    private CKB_Player player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = CKB_Player.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AttackExplosion(Vector3 rangePos, float power, float radius, float upForce)
    {

        // print("Player dead");
        Collider[] colliders = Physics.OverlapSphere(rangePos, radius);
        foreach (Collider body in colliders)
        {
            if (body.tag == "Player")
            {
                Debug.Log(body.name);
                Rigidbody rb = body.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.isKinematic = false;

                    if (noUp)
                    {
                        // print("bomb!!!");
                        rb.AddExplosionForce(power, rangePos, radius, upForce, ForceMode.Impulse);
                        noUp = false;
                    }
                }
            }
        }
        CKB_Player.Instance.Die();
        // CKB_Player.Instance.Die(CKB_Player.DieType.FlyAway);
        // 오류가 나면 이렇게 변경하세요 @!@!
    }
}
