using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class LYJ_MGTurnNeck : MonoBehaviour
{
    private float currentTime;
    private float createTime;

    public bool isUsing;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void TurnNeck()
    {
        StartCoroutine(IETurnNeck());
    }
    
    public void TurnBackNeck()
    {
        StartCoroutine(IETurnBackNeck());
    }
    
    public IEnumerator IETurnBackNeck(float createTime = 1)
    {
        float curTime = 0;
        while (curTime <= createTime)
        {
            curTime += Time.deltaTime;
            gameObject.transform.eulerAngles = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(0, 180, 0), curTime / createTime);
            yield return null;
        }
    }
    
    public IEnumerator IETurnNeck(float createTime = 1)
    {
        float curTime = 0;
        while (curTime <= createTime)
        {
            curTime += Time.deltaTime;
            gameObject.transform.eulerAngles = Vector3.Lerp(new Vector3(0, 180, 0), new Vector3(0, 360, 0), curTime / createTime);
            yield return null;
        }
    }
}
