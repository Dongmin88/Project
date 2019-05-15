using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BGMmgr : MonoBehaviour
{
    public InputField input1;
    public InputField input2;
    // Start is called before the first frame update
    void Start()
    {
        //옵션창
        //게임 배경음 및 게임 효과음 설정
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.name.Equals("BgmSound"))
        {
            soundmgr();
        }
        else if (transform.name.Equals("EffectSound"))
        {
            EffectSound();
        }
    }
    void soundmgr()
    {
        bool a = input1.isFocused;
        if (a)
        {
            transform.GetComponent<Scrollbar>().value = PlayerPrefs.GetFloat("BGMValue");
        }
        else
        {
            PlayerPrefs.SetFloat("BGMValue", transform.GetComponent<Scrollbar>().value);
        }
    }
    void EffectSound()
    {
        bool a = input2.isFocused;
        if (a)
        {
            transform.GetComponent<Scrollbar>().value = PlayerPrefs.GetFloat("EFValue");
        }
        else
        {
            PlayerPrefs.SetFloat("EFValue", transform.GetComponent<Scrollbar>().value);
        }
    }
}
