using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Shot : MonoBehaviour {
    //총알 발사
    float Power;
    public int layermask;
	void Start () {
        Power = 10;
        GetComponent<Rigidbody>().AddForce(transform.forward * 400, ForceMode.Force);
    }
	void Update () {
        ray();
	}
    void ray()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * 0.2f, Color.red, 5f);
        if(Physics.Raycast(transform.position,transform.forward,out hit,0.2f,layermask))
        {
            if (hit.transform.tag == "Player")
            {
                hit.transform.GetComponent<Rigidbody>().AddForce(hit.transform.GetComponent<Rigidbody>().velocity = new Vector3(0, 0.5f, 0) + transform.forward * 0.5f);
                if (hit.transform.name.Equals("Player1"))
                {
                    hit.transform.GetComponent<Player1>().b_hit = true;
                }
                else if (hit.transform.name.Equals("Player2"))
                {
                    hit.transform.GetComponent<Player2>().b_hit = true;
                }
                else if (hit.transform.name.Equals("Player3"))
                {
                    hit.transform.GetComponent<Player3>().b_hit = true;
                }
                else if (hit.transform.name.Equals("Player4"))
                {
                    hit.transform.GetComponent<Player4>().b_hit = true;
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
    }
    /*void OnTriggerStay(Collider other)
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
