using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SupplyDrop : MonoBehaviour
{
    public Transform[] tb;
    public GameObject SBox;
    public GameObject Light;
    float Distance;
    int DropNumber;
    public float CreateDelay;
    public float CreateTime;
    // Start is called before the first frame update
    void Start()
    {
        DropNumber = 4;
        Distance = 5;
        CreateTime = 180;
        DropPosition();
    }
    // Update is called once per frame
    void Update()
    {
        CreateDelay += Time.deltaTime;
        if (CreateDelay > CreateTime)// 보급상자 시간 설정 및 위치조정
        {
            CreateSupplyBox();
            CreateTime += 60;
            CreateTime = (CreateTime >= 300) ? CreateTime -=30 : CreateTime;
            transform.localPosition += new Vector3(0, 10, 0);
        }
    }
    void CreateSupplyBox() // 보급상자 생성
    {
        if (DropNumber == 0)
        {
            DropNumber = 1;
        }
        for (int i = 0; i < DropNumber; i++) {

            if (DropNumber == 4)
            {
                Instantiate(SBox, tb[i]);
                Instantiate(Light, tb[i]);
                tb[i].DetachChildren();
            }
            else
            {
                Instantiate(SBox, tb[i]);
                Instantiate(Light, tb[i]);
                tb[i].DetachChildren();
            }
        }
        Distance = (Distance < 0) ? Distance = 1 : Distance -= 2;
        DropNumber = (DropNumber <= 2) ? DropNumber -= 1 : DropNumber -= 2;
        DropPosition();
    }
    void DropPosition() // 높이 설정에 따른 보급상자 생성 위치 조정.
    {
        if (Distance < 0)
        {
            Distance = 1;
        }
        for (int i = 0; i < tb.Length; i++)
        {
            if (i == 0)
            {
                tb[0].position = transform.position  + transform.forward * Distance;
            }
            else if (i == 1)
            {
                tb[1].position = transform.position + transform.right * Distance;
            }
            else if (i == 2)
            {
                tb[2].position = transform.position +  -transform.right * Distance;
            }
            else if (i == 3)
            {
                tb[3].position = transform.position +  -transform.forward * Distance;
            }
        }
    }
}
