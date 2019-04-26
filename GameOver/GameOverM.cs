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
    }

    // Update is called once per frame
    void Update()
    {
        Rankimage();
    }
    public void Rankimage()
    {
        if (Rank == 1)
        {
            first.SetActive(true);
            Rank = 0;
        }
        else if (Rank == 2)
        {
            second.SetActive(true);
            Rank = 0;
        }
        else if (Rank == 3)
        {
            third.SetActive(true);
            Rank = 0;
        }
        else if (Rank == 4)
        {
            forth.SetActive(true);
            Rank = 0;
        }
    }
}
