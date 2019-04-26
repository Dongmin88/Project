using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpp : MonoBehaviour {
    public GameObject cubess;
    Rigidbody rigid;
    float forc = 20;
	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(0))
        {
            rigid.AddForce(Vector3.up * forc);
        }
	}
}
