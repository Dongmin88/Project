using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cancel_Signdetail : MonoBehaviour
{
    public GameObject Login_detail;
    public GameObject Signup_detail;
    public void Cancel_click()
    {
        Login_detail.SetActive(true);
        Signup_detail.SetActive(false);
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
