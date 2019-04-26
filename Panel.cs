using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Panel : MonoBehaviour
{
    // 독가스 충돌시 작동
    RawImage image;
    float time;
    byte r;
    byte g;
    byte b;
    byte c;
    void Start()
    {
        image = GetComponent<RawImage>();
        if (this.name.Equals("GasEP"))
        {
            r = 255;
            g = 255;
            b = 255;
        }
        else if (this.name.Equals("HpEP"))
        {
            r = 255;
            g = 0;
            b = 0;
            c = 255;
        }
    }
    void Update()
    {
        if (this.name.Equals("GasEP"))
        {
            c = (byte)Random.Range(50, 200);
            time += Time.deltaTime;
            if (time > 0.1f)
            {
                image.color = new Color32(r, g, b, c);
                time = 0;
            }
        }
        else if(this.name.Equals("HpEP"))
        {
            c -= (byte)10;
            image.color = new Color32(r, g, b,c);
            if (c < 20)
            {
                this.gameObject.SetActive(false);
                c = 255;
            }
        }
    }
}
