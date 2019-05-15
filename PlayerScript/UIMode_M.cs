using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIMode_M : MonoBehaviour
{
    Player1 player;
    Player2 player2;
    Player3 player3;
    Player4 player4;
    RawImage raw;
    public Texture Pick;
    public Texture Gun;
    public Texture Create;
    // Use this for initialization
    void Start()
    {
        raw = GetComponent<RawImage>();
        player = FindObjectOfType<Player1>();
        player2 = FindObjectOfType<Player2>();
        player3 = FindObjectOfType<Player3>();
        player4 = FindObjectOfType<Player4>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeImage();
    }
    void ChangeImage() // 서버에서 플레이어 정보를 받아와 이미지 교체
    {
        if (Singleton.instance.Team_number == 1)
        {
            if (player.Mode == 0)
            {
                raw.texture = Pick;
            }
            else if (player.Mode == 1)
            {
                raw.texture = Gun;
            }
            else if (player.Mode == 2)
            {
                raw.texture = Create;
            }
        }
        else if (Singleton.instance.Team_number == 2)
        {
            if (player2.Mode == 0)
            {
                raw.texture = Pick;
            }
            else if (player2.Mode == 1)
            {
                raw.texture = Gun;
            }
            else if (player2.Mode == 2)
            {
                raw.texture = Create;
            }
        }
        else if (Singleton.instance.Team_number == 3)
        {
            if (player3.Mode == 0)
            {
                raw.texture = Pick;
            }
            else if (player3.Mode == 1)
            {
                raw.texture = Gun;
            }
            else if (player3.Mode == 2)
            {
                raw.texture = Create;
            }
        }
        else if (Singleton.instance.Team_number == 4)
        {
            if (player4.Mode == 0)
            {
                raw.texture = Pick;
            }
            else if (player4.Mode == 1)
            {
                raw.texture = Gun;
            }
            else if (player4.Mode == 2)
            {
                raw.texture = Create;
            }
        }
    }
}
