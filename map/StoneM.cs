using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneM : MonoBehaviour
{
    Transform[] Stones;
    public int length;
    // Start is called before the first frame update
    void Start()
    {
        Stones = gameObject.GetComponentsInChildren<Transform>();
        length = Stones.Length;
        Set_Num();
    }

    // Update is called once per frame
    void Update()
    {
        Destroy_Stone();
    }
    public void Destroy_Stone()//오브젝트 파괴시 서버에 정보를 넘겨줌
    {
        if (Singleton.instance.pStone.status == 1)
        {
            DIE_M des_num = Stones[Singleton.instance.pStone.number].GetComponent<DIE_M>();
            Destroy(des_num.gameObject);
            Singleton.instance.Reset_Stone();
        }

    }
    public void Set_Num()// 시작시 오브젝트 번호 설정
    {
        for (int i = 0; i < Stones.Length; i++)
        {
            DIE_M num = Stones[i].GetComponent<DIE_M>();
            if (num != null)
            {
                num.Num = i;
            }
        }
    }
}
