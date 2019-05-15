using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemWindow : MonoBehaviour
{
    public int PlayerNum;
    public RawImage Gun;
    public RawImage Bullet;
    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Check();
    }
    public void Itemsetting(int PlayerNum)
    {
        Gun.transform.GetComponent<GetItem>().PlayerNum = PlayerNum;
        Bullet.transform.GetComponent<GetItem>().PlayerNum = PlayerNum;

        int i_Gun = Random.Range(1,4);
        int i_Bullet = Random.Range(10, 15);

        if (i_Gun == 1)
        {
            Gun.GetComponent<GetItem>().Name = "AK-74m";
            Gun.texture = Resources.Load<Texture>("ak47");
            Gun.transform.GetComponentInChildren<Text>().text = "AK-74m";
            Bullet.GetComponent<GetItem>().Bullet = i_Bullet;
            Bullet.transform.GetComponentInChildren<Text>().text = " X " + i_Bullet.ToString();
        }
        else if (i_Gun ==2)
        {
            Gun.GetComponent<GetItem>().Name = "M4A1_PBR";
            Gun.texture = Resources.Load<Texture>("M4a1");
            Gun.transform.GetComponentInChildren<Text>().text = "M4A1_PBR";
            Bullet.GetComponent<GetItem>().Bullet = i_Bullet;
            Bullet.transform.GetComponentInChildren<Text>().text = " X " + i_Bullet.ToString();
        }
        else if (i_Gun == 3||i_Gun==4)
        {
            Gun.GetComponent<GetItem>().Name = "RevolverM1879";
            Gun.texture = Resources.Load<Texture>("Revolver");
            Gun.transform.GetComponentInChildren<Text>().text = "Revolver";
            Bullet.GetComponent<GetItem>().Bullet = i_Bullet;
            Bullet.transform.GetComponentInChildren<Text>().text = " X " + i_Bullet.ToString();
        }
    }
    void Check()
    {
        if (!Gun.gameObject.active&&!Bullet.gameObject.active)
        {
            gameObject.SetActive(false);
        }
    }
}
