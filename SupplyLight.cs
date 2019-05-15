using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyLight : MonoBehaviour // 보급상자 생성시 같이 생성 위치 보여주기
{
    byte r;
    // Start is called before the first frame update
    void Start()
    {
        r = 200;
        Destroy(gameObject, 10);
    }

    // Update is called once per frame
    void Update()
    {
        float t = 50*Time.deltaTime;
        r -= (byte)t;
        transform.GetComponent<Renderer>().material.color = new Color32(255, 255, 255, r);
        if(r <= 5)
        {
            Destroy(gameObject);
        }
    }
}
