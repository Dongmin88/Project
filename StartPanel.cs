using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StartPanel : MonoBehaviour
{
    Image image;
    byte ColorA;
    void Start()
    {
        ColorA = 255;
        image = GetComponent<Image>();
    }
    void Update()
    {
        Panel();
    }
    void Panel()
    {
        ColorA -= 4;
        image.color = new Color32(0, 0, 0, ColorA);
        if(ColorA <= 10)
        {
            gameObject.SetActive(false);
        }
    }
}
