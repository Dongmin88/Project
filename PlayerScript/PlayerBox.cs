using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBox : MonoBehaviour
{
    Player1 pa1;
    Player2 pa2;
    Player3 pa3;
    Player4 pa4;
    public int Wood;
    public int Stone;
    public int Bullet;
    public int hp;

    // Start is called before the first frame update
    void Start()
    {
        hp = 5;
        pa1 = FindObjectOfType<Player1>();
        pa2 = FindObjectOfType<Player2>();
        pa3 = FindObjectOfType<Player3>();
        pa4 = FindObjectOfType<Player4>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ondie(int Player_number) //플레이어 번호를 넘겨 받아 자원 획득 및 Hp체크
    {
        if (Player_number == 1)
        {
            hp--;
            if (this.transform.tag == "PlayerBox" && hp <= 0)
            {
                pa1.Wood += Wood;
                pa1.Stone += Stone;
                pa1.maxBullet += Bullet;
                StartCoroutine("Destroy_Object");
            }
        }
        if (Player_number == 2)
        {
            hp--;
            if (this.transform.tag == "PlayerBox" && hp <= 0)
            {
                pa2.Wood += Wood;
                pa2.Stone += Stone;
                pa2.maxBullet += Bullet;
                StartCoroutine("Destroy_Object");
            }
        }
        if (Player_number == 3)
        {
            hp--;
            if (this.transform.tag == "PlayerBox" && hp <= 0)
            {
                pa3.Wood += Wood;
                pa3.Stone += Stone;
                pa3.maxBullet += Bullet;
                StartCoroutine("Destroy_Object");
            }
        }
        if (Player_number == 4)
        {
            hp--;
            if (this.transform.tag == "PlayerBox" && hp <= 0)
            {
                pa4.Wood += Wood;
                pa4.Stone += Stone;
                pa4.maxBullet += Bullet;
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
