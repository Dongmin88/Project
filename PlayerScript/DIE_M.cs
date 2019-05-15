using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DIE_M : MonoBehaviour
{
    public int Num;
    Player1 Pa;
    Player2 Pa2;
    Player3 Pa3;
    Player4 Pa4;
    public int survive;//0 죽음, 1 살아있음
    int hp = 10;
    float Wood;
    float Stone;
    // Use this for initialization
    void Start()
    {
        Wood = 10;
        Stone = 5;
        survive = 1;
        Pa = FindObjectOfType<Player1>();
        Pa2 = FindObjectOfType<Player2>();
        Pa3 = FindObjectOfType<Player3>();
        Pa4 = FindObjectOfType<Player4>();
        if (this.transform.tag == "BulletBox")
        {
            hp = 5;
        }
    }
    // Update is called once per frame
    void Update()
    {
    }
    public void ondie(int Player_number) // 플레이어 번호를 받아와 데미지 및 사망 체크
    {
        if (Player_number == 1)
        {
            hp--;
            if (this.transform.tag == "wood")
            {
                Pa.Wood += (int)Wood;
                Pa.GetMaterial(Wood,"Wood");
            }
            if (this.transform.tag == "stone")
            {
                Pa.Stone += (int)Stone;
                Pa.GetMaterial(Stone,"Stone");
            }
            if (this.transform.tag == "BulletBox" && hp <= 0)
            {
                Pa.ItemWindow.GetComponent<ItemWindow>().Bullet.gameObject.SetActive(true);
                Pa.ItemWindow.GetComponent<ItemWindow>().Gun.gameObject.SetActive(true);
                Pa.ItemWindow.GetComponent<ItemWindow>().Itemsetting(Player_number);
                Pa.ItemWindow.SetActive(true);
            }
            if (hp <= 0)
            {
                if (this.transform.tag == "wood" && survive == 1)
                {
                    survive = 0;
                    Singleton.instance.Send_Tree(Num);
                }
                else if (this.transform.tag == "stone" && survive == 1)
                {
                    survive = 0;
                    Singleton.instance.Send_Stone(Num);
                }
                else if (this.transform.tag == "BulletBox" && survive == 1)
                {
                    survive = 0;
                    Singleton.instance.Send_Bullet(Num);
                }
                StartCoroutine("Destroy_Object");
            }
        }
        if (Player_number == 2)
        {
            hp--;
            if (this.transform.tag == "wood")
            {
                Pa2.Wood += (int)Wood;
                Pa2.GetMaterial(Wood, "Wood");
            }
            if (this.transform.tag == "stone")
            {
                Pa2.Stone += (int)Stone;
                Pa2.GetMaterial(Stone, "Stone");
            }
            if (this.transform.tag == "BulletBox" && hp <= 0)
            {
                Pa2.ItemWindow.GetComponent<ItemWindow>().PlayerNum = Player_number;
                Pa2.ItemWindow.GetComponent<ItemWindow>().Bullet.gameObject.SetActive(true);
                Pa2.ItemWindow.GetComponent<ItemWindow>().Gun.gameObject.SetActive(true);
                Pa2.ItemWindow.GetComponent<ItemWindow>().Itemsetting(Player_number);
                Pa2.ItemWindow.SetActive(true);
            }
            if (hp <= 0)
            {
                if (this.transform.tag == "wood" && survive == 1)
                {
                    survive = 0;
                    Singleton.instance.Send_Tree(Num);
                }
                else if (this.transform.tag == "stone" && survive == 1)
                {
                    survive = 0;
                    Singleton.instance.Send_Stone(Num);
                }
                else if (this.transform.tag == "BulletBox" && survive == 1)
                {
                    survive = 0;
                    Singleton.instance.Send_Bullet(Num);
                }
                StartCoroutine("Destroy_Object");
            }
        }
        if (Player_number == 3)
        {
            hp--;
            if (this.transform.tag == "wood")
            {
                Pa3.Wood += (int)Wood;
                Pa3.GetMaterial(Wood, "Wood");
            }
            if (this.transform.tag == "stone")
            {
                Pa3.Stone += (int)Stone;
                Pa3.GetMaterial(Stone,"Stone");
            }
            if (this.transform.tag == "BulletBox" && hp <= 0)
            {
                Pa3.ItemWindow.GetComponent<ItemWindow>().PlayerNum = Player_number;
                Pa3.ItemWindow.GetComponent<ItemWindow>().Bullet.gameObject.SetActive(true);
                Pa3.ItemWindow.GetComponent<ItemWindow>().Gun.gameObject.SetActive(true);
                Pa3.ItemWindow.GetComponent<ItemWindow>().Itemsetting(Player_number);
                Pa3.ItemWindow.SetActive(true);
            }
            if (hp <= 0)
            {
                if (this.transform.tag == "wood" && survive == 1)
                {
                    survive = 0;
                    Singleton.instance.Send_Tree(Num);
                }
                else if (this.transform.tag == "stone" && survive == 1)
                {
                    survive = 0;
                    Singleton.instance.Send_Stone(Num);
                }
                else if (this.transform.tag == "BulletBox" && survive == 1)
                {
                    survive = 0;
                    Singleton.instance.Send_Bullet(Num);
                }
                StartCoroutine("Destroy_Object");
            }
        }
        if (Player_number == 4)
        {
            hp--;
            if (this.transform.tag == "wood")
            {
                Pa4.Wood += (int)Wood;
                Pa4.GetMaterial(Wood,"Wood");
            }
            if (this.transform.tag == "stone")
            {
                Pa4.Stone += (int)Stone;
                Pa4.GetMaterial(Stone,"Stone");
            }
            if (this.transform.tag == "BulletBox" && hp <= 0)
            {
                Pa4.ItemWindow.GetComponent<ItemWindow>().PlayerNum = Player_number;
                Pa4.ItemWindow.GetComponent<ItemWindow>().Bullet.gameObject.SetActive(true);
                Pa4.ItemWindow.GetComponent<ItemWindow>().Gun.gameObject.SetActive(true);
                Pa4.ItemWindow.GetComponent<ItemWindow>().Itemsetting(Player_number);
                Pa4.ItemWindow.SetActive(true);
            }
            if (hp <= 0)
            {
                if (this.transform.tag == "wood" && survive == 1)
                {
                    survive = 0;
                    Singleton.instance.Send_Tree(Num);
                }
                else if (this.transform.tag == "stone" && survive == 1)
                {
                    survive = 0;
                    Singleton.instance.Send_Stone(Num);
                }
                else if (this.transform.tag == "BulletBox" && survive == 1)
                {
                    survive = 0;
                    Singleton.instance.Send_Bullet(Num);
                }
                StartCoroutine("Destroy_Object");
            }
        }
    }
    IEnumerator Destroy_Object()
    {
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
    }
}