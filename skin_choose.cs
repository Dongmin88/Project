using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skin_choose : MonoBehaviour
{

    public GameObject skin;
    void start()
    {
         
    }

    public void skinclick()
    {
        transform.Translate(Vector3.forward, Space.Self);
        transform.Translate(new Vector3(0, 0, 0.1f));
    }
}
