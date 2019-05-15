using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheck : MonoBehaviour
{
    public float Hp;
    void Start()
    {
        transform.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("EFValue");
        transform.GetComponent<AudioSource>().Play();
        string Type = transform.name; // 타입에 따라 HP 부여
        Type = Type.Substring(0,2);
        if (Type == "WW")
        {
            Hp = 20;
        }
        else if (Type == "WS")
        {
            Hp = 30;
        }
    }
    // Update is called once per frame
    void Update()
    {
        Die();
    }
    void Die()
    {
        if (Hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
