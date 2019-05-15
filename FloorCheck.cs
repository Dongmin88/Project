using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCheck : MonoBehaviour
{
    public  bool Check;
    public float Hp;
    string Type;
    byte r;
    void Start()
    {
        transform.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("EFValue");
        transform.GetComponent<AudioSource>().Play();
        Check = false;
        Type = transform.name;
        Type = Type.Substring(0, 2); // 바닥 오브젝트 타입별 HP 부여
        if(Type == "WF")
        {
            Hp = 30;
            transform.GetComponent<Renderer>().material.color = new Color32(163, 97, 37, 0);
        }
        else if (Type == "SF")
        {
            Hp = 50;
            transform.GetComponent<Renderer>().material.color = new Color32(255, 255, 255, 0);
        }
    }
    void Update()
    {
            
        if (Type == "WF"&&r<255) // 오브젝트 생성 효과
        {
           r += (byte)5;
           transform.GetComponent<Renderer>().material.color = new Color32(163, 97, 37, r);
        }
        else if (Type == "SF"&&r<255) //""
        {
           r += (byte)5;
           transform.GetComponent<Renderer>().material.color = new Color32(255, 255, 255, r);
        }
        destroyfloor();
    }
    public void onRay() // 바닥 설치시 레이를 위로 쏴 있으면 히트 오브젝트 삭제
    {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position,this.transform.up, out hit,1f,9)) // 위로 레이 발사
        {
            Destroy(hit.transform.gameObject);
            Check = true;
        }
        else
        {
            Check = false;
        }
    }
    void destroyfloor()
    {
        if (Hp <= 0)
        {
            float time = 0;
            Destroy(gameObject);
        }
    }
}
