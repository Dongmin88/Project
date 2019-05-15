using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBoxM : MonoBehaviour
{
    public GameObject Woodobj;
    public GameObject stoneobj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        structure_Box();
    }
    public void structure_Box()
    {
        if (Singleton.instance.CreateBox_Floor.kind == 1)
        {
            GameObject WF = Instantiate(Woodobj);
            WF.transform.position = new Vector3(Singleton.instance.CreateBox_Floor.fx, Singleton.instance.CreateBox_Floor.fy, Singleton.instance.CreateBox_Floor.fz);
            Singleton.instance.Reset_Floor();
        }
        else if (Singleton.instance.CreateBox_Floor.kind == 2)
        {
            GameObject SF=Instantiate(stoneobj);
            SF.transform.position = new Vector3(Singleton.instance.CreateBox_Floor.fx, Singleton.instance.CreateBox_Floor.fy, Singleton.instance.CreateBox_Floor.fz);
            Singleton.instance.Reset_Floor();
        }
    }
}
