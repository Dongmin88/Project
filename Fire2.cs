﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire2 : MonoBehaviour
{
    SpriteRenderer SR;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 0.5f);
        SR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale += new Vector3(0.05f, 0.04f, 0.05f) * Time.deltaTime * 1.5f;
        SR.color -= new Color32(0, 0, 0, 10);
        transform.position -= transform.forward * Time.deltaTime * 0.03f;
        transform.localRotation *= Quaternion.Euler(new Vector3(0, 0, 30));
    }
}
