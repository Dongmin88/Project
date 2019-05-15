using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TextMove : MonoBehaviour
{
    // Start is called before the first frame update
    public float Num;
    void Start()
    {
        Destroy(gameObject,1);
        transform.GetComponent<Text>().text = " + " + (int)Num;
    }

    // Update is called once per frame
    void Update()
    {
        Moves();
    }
    void Moves()
    {
        transform.GetComponent<RectTransform>().position += new Vector3(0, 10f * Time.deltaTime);
    }
}
