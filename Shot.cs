using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Shot : MonoBehaviour {
    //총알 발사
    float Power;
    float Range;
    public int layermask;
    public string Gun;
    Transform tr;
	void Start () {

        tr = transform;
        transform.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("EFValue");
        transform.GetComponent<AudioSource>().Play();
        layermask += (-1) - (1 << 14) + (-1)-(1<<gameObject.layer);
        if (transform.name.Equals("AK-74m")||transform.name.Equals("M4A1_PBR")) // 총기 정보를 받아와 데미지 및 사정거리 설정
        {
            Range = 15;
            Power = 5;
            if (transform.name.Equals("AK-74m"))
            {
                Power += 2;
            }
        }
        else if(transform.name.Equals("RevolverM1879"))
        {
            Range = 10;
            Power = 15;
        }
        ray();
        Destroy(gameObject,0.1f);
    }
	void Update () {
	}
    void ray() // 총알 피격 체크
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position,transform.forward,out hit, Range, layermask))
        {
            if (hit.transform.tag == "Player")
            {
                if (hit.transform.name.Equals("Player1"))
                {
                    hit.transform.GetComponent<Player1>().b_hit = true;
                    hit.transform.GetComponent<Player1>().hp -= Power;
                    if(hit.transform.GetComponent<Player1>().hp <= 0)
                    {
                        hit.transform.GetComponent<Player1>().GetComponent<Rigidbody>().velocity = new Vector3(0, 1, 0)+ tr.transform.forward * 1;
                    }
                }
                else if (hit.transform.name.Equals("Player2"))
                {
                    hit.transform.GetComponent<Player2>().b_hit = true;
                    hit.transform.GetComponent<Player2>().hp -= Power;
                    if (hit.transform.GetComponent<Player2>().hp <= 0)
                    {
                        hit.transform.GetComponent<Player2>().GetComponent<Rigidbody>().velocity = new Vector3(0, 1, 0) + tr.transform.forward * 1;
                    }
                }
                else if (hit.transform.name.Equals("Player3"))
                {
                    hit.transform.GetComponent<Player3>().b_hit = true;
                    hit.transform.GetComponent<Player3>().hp -= Power;
                    if (hit.transform.GetComponent<Player3>().hp <= 0)
                    {
                        hit.transform.GetComponent<Player3>().GetComponent<Rigidbody>().velocity = new Vector3(0, 1, 0) +  tr.transform.forward * 1;
                    }
                }
                else if (hit.transform.name.Equals("Player4"))
                {
                    hit.transform.GetComponent<Player4>().b_hit = true;
                    hit.transform.GetComponent<Player4>().hp -= Power;
                    if (hit.transform.GetComponent<Player4>().hp <= 0)
                    {
                        hit.transform.GetComponent<Player4>().GetComponent<Rigidbody>().velocity = new Vector3(0, 1, 0) +tr.transform.forward * 1;
                    }
                }
            }
            else if (hit.transform.tag == "floor")
            {
                hit.transform.GetComponent<FloorCheck>().Hp -= Power;
            }
            else if (hit.transform.tag == "wall")
            {
                hit.transform.GetComponent<WallCheck>().Hp -= Power;
            }
            Destroy(gameObject);
        }
    }  /*void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player"&&!name.Equals(other.transform.name)) // 03.04 추가 
        {
            other.GetComponent<Rigidbody>().AddForce(other.GetComponent<Rigidbody>().velocity = new Vector3(0, 0.1f, 0) + transform.forward * 0.1f);
            if (other.name.Equals("Player1"))
            {
                other.GetComponent<Player1>().hp -= Power;
            }
            else if (other.name.Equals("Player2"))
            {
                other.GetComponent<Player2>().hp -= Power;
            }
            else if (other.name.Equals("Player3"))
            {
                other.GetComponent<Player3>().hp -= Power;
            }
            else if (other.name.Equals("Player4"))
            {
                other.GetComponent<Player4>().hp -= Power;
            }
        }
        else if (other.tag == "floor")
        {
            other.GetComponent<FloorCheck>().Hp -= Power;
        }
        else if(other.tag == "wall")
        {
            other.GetComponent<WallCheck>().Hp -= Power;
        }
        if (!other.transform.name.Equals(name)&&other.tag != "Gas")
        {
            Destroy(gameObject);
        }
    }*/
}
