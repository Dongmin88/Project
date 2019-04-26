using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Signup : MonoBehaviour
{
    public GameObject Login_detail;
    public GameObject Signup_detail;
    public InputField inID;
    public InputField inPass;
    public GameObject signupsuccess;
    public GameObject signupfail;
    public Button Sign_up;

    public void Signup_Click()
    {
        string id = inID.text;
        string pass = inPass.text;
        string sign = Singleton.instance.Signup_Webserver(id, pass);
        if (sign == "fail")
        {
            signupfail.SetActive(true);
            Sign_up.interactable = false;
            StartCoroutine("buttonablefail");

            //실패시 뜰 ui 생성
        }
        if (sign != "fail")
        {
            signupsuccess.SetActive(true);
            Sign_up.interactable = false;
            StartCoroutine("buttonablesuccess");
        }
    }
    IEnumerator buttonablefail()
    {
        yield return new WaitForSeconds(3.0f);
        Sign_up.interactable = true;
        signupfail.SetActive(false);
    }
    IEnumerator buttonablesuccess()
    {
        yield return new WaitForSeconds(3.0f);
        Sign_up.interactable = true;
        signupsuccess.SetActive(false);
        Login_detail.SetActive(true);
        Signup_detail.SetActive(false);
    }
}
