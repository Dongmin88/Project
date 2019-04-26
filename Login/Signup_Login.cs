using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signup_Login : MonoBehaviour
{
    public GameObject Login_detail;
    public GameObject Signup_detail;

    public void Signup_login_click()
    {
        Login_detail.SetActive(false);
        Signup_detail.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
