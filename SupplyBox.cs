using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyBox : MonoBehaviour
{
    public int Wood;
    public int Stone;
    public int Bullet;
    bool arrive;
    public float Hp;
    public int PlayerNum;
    Player1 p1;
    Player2 p2;
    Player3 p3;
    Player4 p4;
    public GameObject gb;
    private void Start()
    {
        Hp = 5;
        Wood = 100;
        Stone = 50;
        Bullet = 50;
        arrive = false;
        p1 = FindObjectOfType<Player1>();
        p2 = FindObjectOfType<Player2>();
        p3 = FindObjectOfType<Player3>();
        p4 = FindObjectOfType<Player4>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!arrive)
        {
            PositionUpdate();//부딪히기 전 까지 천천히 하강
        }
        else
        {
            onDie(); // 떨어진 이후 Hp체크
        }
    }
    void PositionUpdate()
    {
        gb.transform.localPosition -= new Vector3(0,0.5f*Time.deltaTime, 0);
    }
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "floor" || other.tag == "Map" ||other.tag=="Player")
        {
            arrive = true;
        }
    }
    public void onDemage(int PN)
    {
        PlayerNum = PN;
        Hp -= 1;
    }
    void onDie()
    {
        if (Hp <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        if (PlayerNum == 1)
        {
            p1.Wood += Wood;
            p1.Stone += Stone;
            p1.maxBullet += Bullet;
            p1.GetMaterial(Wood,"Wood");
            p1.GetMaterial(Stone,"Stone");
            p1.GetMaterial(Bullet,"Bullet");
        }
        else if (PlayerNum == 2)
        {
            p2.Wood += Wood;
            p2.Stone += Stone;
            p2.maxBullet += Bullet;
            p2.GetMaterial(Wood, "Wood");
            p2.GetMaterial(Stone, "Stone");
            p2.GetMaterial(Bullet, "Bullet");
        }
        else if (PlayerNum == 3)
        {
            p3.Wood += Wood;
            p3.Stone += Stone;
            p3.maxBullet += Bullet;
            p3.GetMaterial(Wood, "Wood");
            p3.GetMaterial(Stone, "Stone");
            p3.GetMaterial(Bullet, "Bullet");
        }
        else if (PlayerNum == 4)
        {
            p4.Wood += Wood;
            p4.Stone += Stone;
            p4.maxBullet += Bullet;
            p4.GetMaterial(Wood, "Wood");
            p4.GetMaterial(Stone, "Stone");
            p4.GetMaterial(Bullet, "Bullet");
        }
    }
}
