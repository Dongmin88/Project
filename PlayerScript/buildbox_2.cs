using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildbox_2 : MonoBehaviour
{

    public bool Check;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
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
    public void Rays()
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
        else if (Physics.Raycast(transform.position, -transform.forward, out hit, 0.5f, 9))
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
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "floor")
        {
            Check = false;
        }
    }
}
