using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageC : MonoBehaviour
{
    public GameObject Image;
    public bool imagechage;
    Vector3 imagescale;
    // Start is called before the first frame update
    void Start()
    {
        imagechage = false;
    }

    // Update is called once per frame
    void Update()
    {
        changescale();
    }
    void changescale()
    {
        if (imagechage == false)
        {
            Image.transform.localScale += new Vector3(0, -0.03f * Time.deltaTime, 0);
            imagescale = Image.transform.localScale;
            if (imagescale.y <= 0.9f)
            {
                imagechage = true;
            }
        }
        else if (imagechage == true)
        {
            Image.transform.localScale += new Vector3(0, 0.03f * Time.deltaTime, 0);
            imagescale = Image.transform.localScale;
            if (imagescale.y >= 1)
            {
                imagechage = false;
            }
        }
    }
}
