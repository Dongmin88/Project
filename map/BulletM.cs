using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletM : MonoBehaviour
{
    Transform[] bullets;
    public int length;
    // Start is called before the first frame update
    void Start()
    {
        bullets = gameObject.GetComponentsInChildren<Transform>();
        length = bullets.Length;
        Set_Num();
    }

    // Update is called once per frame
    void Update()
    {
        Destroy_bullet();
    }
    public void Destroy_bullet()
    {
        if (Singleton.instance.pBullet.status == 1)
        {
            DIE_M des_num = bullets[Singleton.instance.pBullet.number].GetComponent<DIE_M>();
            Destroy(des_num.gameObject);
            Singleton.instance.Reset_Bullet();

        }

    }
    public void Set_Num()
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            DIE_M num = bullets[i].GetComponent<DIE_M>();
            if (num != null)
            {
                num.Num = i;
            }
        }
    }
}