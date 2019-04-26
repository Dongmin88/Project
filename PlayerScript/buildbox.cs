using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildbox : MonoBehaviour
{
    public bool Check;
    void Update()
    {
        if (Check) // 바닥 오브젝트 설치 가능 하면 블루 불가능 레드
        {
            gameObject.GetComponent<Renderer>().material = Resources.Load<Material>("BuildBox1"); // 블루
        }
        else
        {
            gameObject.GetComponent<Renderer>().material = Resources.Load<Material>("BuildBox2"); // 레드
        }
    }
    public void Rays() // 바닥 오브젝트 설치 가능 체크
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 0.1f, 9))
        {
            Check = true;
        }
        else if (Physics.Raycast(transform.position, transform.forward, out hit, 0.5f, 9))
        {
            Check = true;
        }
        else if(Physics.Raycast(transform.position,-transform.forward,out hit, 0.5f, 9))
        {
            Check = true;
        }
        else if (Physics.Raycast(transform.position, transform.right, out hit, 0.5f, 9))
        {
            Check = true;
        }
        else if (Physics.Raycast(transform.position, -transform.right, out hit, 0.5f, 9))
        {
            Check = true;
        }
        else
        {
            Check = false;
        }
    }
}
