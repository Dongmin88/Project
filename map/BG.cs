using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG : MonoBehaviour
{
    private void Start()
    {
        transform.GetComponent<AudioSource>().Play();
    }
    void Update()
    {
        transform.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("BGMValue");
    }
}
