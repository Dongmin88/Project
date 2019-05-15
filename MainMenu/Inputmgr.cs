using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Inputmgr : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //옵셩창
        //게임 배경 및 게임 효과음 설정
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.name.Equals("BgmInput"))
        {
            BgmValue();
        }
        else if (transform.name.Equals("EffectInput"))
        {
            EffectValue();
        }
    }
    void BgmValue()
    {
        bool a = transform.GetComponent<InputField>().isFocused;
        if (a)
        {
            PlayerPrefs.SetFloat("BGMValue", float.Parse(transform.GetComponent<InputField>().text.ToString()) / 100);
        }
        else
        {
            transform.GetComponent<InputField>().text = (Mathf.Round(PlayerPrefs.GetFloat("BGMValue") * 100)).ToString();
        }
    }
    void EffectValue()
    {
        bool a = transform.GetComponent<InputField>().isFocused;
        if (a)
        {
            PlayerPrefs.SetFloat("EFValue", float.Parse(transform.GetComponent<InputField>().text.ToString()) / 100);
        }
        else
        {
            transform.GetComponent<InputField>().text = (Mathf.Round(PlayerPrefs.GetFloat("EFValue") * 100)).ToString();
        }
    }
}
