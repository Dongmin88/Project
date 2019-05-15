using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Gas : MonoBehaviour {

    //독가스
    ParticleSystem Ps;
    Player1 p1;
    Player2 p2;
    Player3 p3;
    Player4 p4;
    Vector3 gas;
    Vector3 Gaspt;
    public float GasSpeed;
    public float Times;
    float Delay;
    float Gasheight;
    int Phase;
    // Use this for initialization
    void Start () {
        Ps = transform.GetComponentInChildren<ParticleSystem>();
        Delay = 150;
        Phase = 1;
        Gasheight = 10;
        p1 = FindObjectOfType<Player1>();
        p2 = FindObjectOfType<Player2>();
        p3 = FindObjectOfType<Player3>();
        p4 = FindObjectOfType<Player4>();
    }

    // Update is called once per frame
    void Update()
    {
        GasTimes();
    }
    void GasTimes()
    {
        Times += Time.deltaTime;
        if (Times > Delay) // 시간이 지날수록 가스 높이 설정 및 시간 단축
        {
            gas = transform.localScale;
            gas.y += GasSpeed * Time.deltaTime;
            transform.localScale = gas;
            Ps.transform.localScale = new Vector3(Ps.transform.localScale.x, transform.localScale.y - 0.6f, Ps.transform.localScale.z);
            if (gas.y > Gasheight)
            {
                GasPhase();
                Phase++;
                Delay -=30;
                Times = 0;
                Gasheight += 10;
            }
        }
    }
    void GasPhase() // 가스데미지 설정
    {
        if (Phase == 1)
        {
            p1.GasDam = 1;
            p2.GasDam = 1;
            p3.GasDam = 1;
            p4.GasDam = 1;
        }
        else if (Phase == 2)
        {
            p1.GasDam = 5;
            p2.GasDam = 5;
            p3.GasDam = 5;
            p4.GasDam = 5;
        }
        else if (Phase == 3)
        {
            p1.GasDam = 10;
            p2.GasDam = 10;
            p3.GasDam = 10;
            p4.GasDam = 10;
        }
        else if (Phase == 4)
        {
            p1.GasDam = 20;
            p2.GasDam = 20;
            p3.GasDam = 20;
            p4.GasDam = 20;
        }
    }
}
