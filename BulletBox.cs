using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBox : MonoBehaviour
{
    public Material box1;
    public Material box2;
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player"&&other.gameObject.layer==9)
        {
            transform.GetComponent<Renderer>().material = box2;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        transform.GetComponent<Renderer>().material = box1;
    }
}
