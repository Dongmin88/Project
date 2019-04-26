﻿using System.Collections;
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
    // Use this for initialization
    void Start()
    {
        Pa = FindObjectOfType<Player1>();
        Pa2 = FindObjectOfType<Player2>();
        Pa3 = FindObjectOfType<Player3>();
        Pa4 = FindObjectOfType<Player4>();
    }
    // Update is called once per frame
    void Update()
    {
    }
    public void ondie(int Player_number)
    {
        if (Player_number == 1)
        {
            hp--;
            if (this.transform.tag == "wood")
            {
                Pa.Wood += 10;
            }
            if (this.transform.tag == "stone")
            {
                Pa.Stone += 5;
            }
            if (this.transform.tag == "BulletBox" && hp <= 0)
            {
                Pa.maxBullet += 5;
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
                Destroy(gameObject);
            }
        }
        if (Player_number == 2)
        {
            hp--;
            if (this.transform.tag == "wood")
            {
                Pa2.Wood += 10;
            }
            if (this.transform.tag == "stone")
            {
                Pa2.Stone += 5;
            }
            if (this.transform.tag == "BulletBox" && hp <= 0)
            {
                Pa2.maxBullet += 5;
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
                Destroy(gameObject);
            }
        }
        if (Player_number == 3)
        {
            hp--;
            if (this.transform.tag == "wood")
            {
                Pa3.Wood += 10;
            }
            if (this.transform.tag == "stone")
            {
                Pa3.Stone += 5;
            }
            if (this.transform.tag == "BulletBox" && hp <= 0)
            {
                Pa3.maxBullet += 5;
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
                Destroy(gameObject);
            }
        }
        if (Player_number == 4)
        {
            hp--;
            if (this.transform.tag == "wood")
            {
                Pa4.Wood += 10;
            }
            if (this.transform.tag == "stone")
            {
                Pa4.Stone += 5;
            }
            if (this.transform.tag == "BulletBox" && hp <= 0)
            {
                Pa4.maxBullet += 5;
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
                Destroy(gameObject);
            }
        }
    }
}