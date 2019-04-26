using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class soundmgr : MonoBehaviour
{
    public Slider sound;
    public AudioSource audio;
    float soundvalue = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        soundvalue = PlayerPrefs.GetFloat("soundval", 1.0f);
        sound.value = soundvalue;
        audio.volume = sound.value;
    }

    // Update is called once per frame
    void Update()
    {
        SliderMgr();
    }

    void SliderMgr()
    {
        audio.volume = sound.value;
        soundvalue = sound.value;
        soundvalue = PlayerPrefs.GetFloat("soundval", soundvalue);
    }
}