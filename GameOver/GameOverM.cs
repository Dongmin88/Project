using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverM : MonoBehaviour
{
    public int Rank;//등수
    public GameObject first;
    public GameObject second;
    public GameObject third;
    public GameObject forth;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Rank = Singleton.instance.Rank;
        if (Rank == 1)
        {
            Singleton.instance.GameWin_Webserver(Singleton.instance.Userinfo.id, Singleton.instance.Userinfo.Win);
        }
        else if (Rank != 1)
        {
            Singleton.instance.GameOver_Webserver(Singleton.instance.Userinfo.id, Singleton.instance.Userinfo.Lose);
        }
        Rankimage();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Rankimage()
    {
        if (Rank == 1)
        {
            first.SetActive(true);
        }
        else if (Rank == 2)
        {
            second.SetActive(true);
        }
        else if (Rank == 3)
        {
            third.SetActive(true);
        }
        else if (Rank == 4)
        {
            forth.SetActive(true);
        }
    }
}
