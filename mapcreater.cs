using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapcreater : MonoBehaviour {

    public GameObject cube;
    public GameObject plane;

    public float cubeX,cubeY;

	void Start () {
        //for (cubeX = 0; cubeX < 20; cubeX++)
        //{
        //    Instantiate(plane, new Vector3(2 * cubeX, 0, 0), Quaternion.identity);
        //    for(cubeY=0;cubeY < 20; cubeY++)
        //    {
        //       Instantiate(plane, new Vector3(2 * cubeX, 0, 2 * cubeY), Quaternion.identity);
        //    }
        //}

        Instantiate(cube, new Vector3(0, 0, 0), Quaternion.identity);
        Instantiate(cube, new Vector3(0.4f, 1, 5), Quaternion.identity);
        Instantiate(cube, new Vector3(0.61f, 1, 0.8f), Quaternion.identity);
        Instantiate(cube, new Vector3(1.2f, 0, 4), Quaternion.identity);

    }
	

	void Update () {

  
    }
}
