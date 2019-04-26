using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class LoginClick : MonoBehaviour
{
    public InputField inID;
    public InputField inPass;
    public GameObject Loginfail;
    public Button Login;
    public Button Signup;

    public void Login_Click()
    {
        string id = inID.text;
        string pass = inPass.text;
        string login = Singleton.instance.Login_Webserver(id, pass);
        if (login == "fail")
        {
            Login_Fail();
            //실패시 뜰 ui 생성
        }
        if (login != "fail")
        {
            Singleton.instance.Login_success(login);
            Singleton.instance.Send_LoginMSG();
            Singleton.instance.Receive_MSG();
            StartCoroutine("logincheck");
        }
    }

    IEnumerator buttonable()
    {
        yield return new WaitForSeconds(3.0f);
        Login.interactable = true;
        Signup.interactable = true;
        Loginfail.SetActive(false);
    }
    IEnumerator loginsuccess()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("MainMenu");
    }
    IEnumerator logincheck()
    {
        yield return new WaitForSeconds(1);
        if (Singleton.instance.Login.status == 1)
        {
            StartCoroutine("loginsuccess");
        }
        if (Singleton.instance.Login.status != 1)
        {
            Login_Fail();
        }
    }
    public void Login_Fail()
    {
        Loginfail.SetActive(true);
        Login.interactable = false;
        Signup.interactable = false;
        StartCoroutine("buttonable");
    }
}
