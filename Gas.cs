using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gas : MonoBehaviour {

    //독가스
    Vector3 gas;
    public float GasSpeed;
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        gas = transform.localScale;
        gas.y += GasSpeed*Time.deltaTime;
        transform.localScale = gas;
    }
}
