using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public Text rankView;
    public int rank;

    void Start()
    {
        rank = 2;
        RankText();
    }

    void Update()
    {
    }
    void RankText()
    {
        rankView.text = rankView.text +" "+ rank.ToString();
    }
    public void mainclick()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
