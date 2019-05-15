using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GetItem : MonoBehaviour
{
    public string Name;
    public int Bullet;
    public int PlayerNum;
    Player1 player1;
    Player2 player2;
    Player3 player3;
    Player4 player4;
    private void Start()
    {
        player1 = FindObjectOfType<Player1>();
        player2 = FindObjectOfType<Player2>();
        player3 = FindObjectOfType<Player3>();
        player4 = FindObjectOfType<Player4>();
    }
    void Update()
    {
        
    }
    public void BulletTouch() // 버튼 이벤트 발생시 함수 작동 총알 획득
    {
        if (PlayerNum == 1)
        {
            player1.maxBullet += Bullet;
            player1.GetMaterial((float)Bullet,"Bullet");
        }
        else if (PlayerNum == 2)
        {
            player2.maxBullet += Bullet;
            player2.GetMaterial((float)Bullet,"Bullet");
        }
        else if (PlayerNum == 3)
        {
            player3.maxBullet += Bullet;
            player3.GetMaterial((float)Bullet,"Bullet");
        }
        else if (PlayerNum == 4)
        {
            player4.maxBullet += Bullet;
            player4.GetMaterial((float)Bullet,"Bullet");
        }
        gameObject.SetActive(false);
    }
    public void GunTouch() // 버튼 이벤트 발생시 함수 작동 총기 획득 / 획득시 장전 총알 초기화
    {
        if (PlayerNum == 1)
        {
            player1.Guns = Name;
            player1.maxBullet += player1.minBullet;
            player1.minBullet = 0;
        }
        else if (PlayerNum == 2)
        {
            player2.Guns = Name;
            player2.maxBullet += player2.minBullet;
            player2.minBullet = 0;
        }
        else if (PlayerNum == 3)
        {
            player3.Guns = Name;
            player3.maxBullet += player3.minBullet;
            player3.minBullet = 0;
        }
        else if (PlayerNum == 4)
        {
            player4.Guns = Name;
            player4.maxBullet += player4.minBullet;
            player4.minBullet = 0;
        }
        gameObject.SetActive(false);
    }
}
