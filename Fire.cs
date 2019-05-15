using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 0.4f);
    }

    // Update is called once per frame
    void Update() // 총을 쏠 시에 앞으로 나아가면서 회전 합니다.( 총구화염 이펙트)
    {
        transform.localPosition += new Vector3(0,0, 00.05f * Time.deltaTime);
        transform.localRotation *= Quaternion.Euler(new Vector3(50, 0, 0));
    }
}
