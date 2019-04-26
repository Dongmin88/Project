using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition -= new Vector3(0, 0.05f * Time.deltaTime, 0);
        transform.localRotation *= Quaternion.Euler(new Vector3(50, 0, 0));
    }
}
