using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WallbuildBox : MonoBehaviour
{
    public bool Check;

    private void Start()
    {
        Check = true;
    }
    void Update()
    {
        Checker();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "wall"||other.tag=="floor")
        {
            Check = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Check = true;
    }
    void Checker() // 불값을 이용하여 벽 오브젝트 설치 체크
    {
        if (Check)
        {
            gameObject.GetComponent<Renderer>().material = Resources.Load<Material>("BuildBox1");
        }
        else
        {
            gameObject.GetComponent<Renderer>().material = Resources.Load<Material>("BuildBox2");
        }
    }
}
