using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ImageMove : MonoBehaviour
{
    Text tx;
    public RectTransform Cv;
    public Text Num;
    public float FNum;
    void Start()
    {
        string name = transform.name.Substring(0, 2);
        if (name.Equals("Wo"))
        {
            tx = GameObject.FindGameObjectWithTag("WoodText").GetComponent<Text>();
            transform.GetComponent<RawImage>().texture = Resources.Load<Texture>("wood_tex");
            Destroy(gameObject, 0.9f);
        }
        else if (name.Equals("St"))
        {
            tx = GameObject.FindGameObjectWithTag("StoneText").GetComponent<Text>();
            transform.GetComponent<RawImage>().texture = Resources.Load<Texture>("stone_tex");
            Destroy(gameObject, 0.9f);
        }
        else if (name.Equals("Bu"))
        {
            tx = GameObject.FindGameObjectWithTag("BulletText").GetComponent<Text>();
            transform.GetComponent<RawImage>().texture = Resources.Load<Texture>("556mm");
            Destroy(gameObject, 0.8f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    void Move()
    {
        if (tx != null)
        {
            transform.localScale -= new Vector3(0.5f*Time.deltaTime, 0.5f*Time.deltaTime, 0.5f*Time.deltaTime);
            if (tx.tag == "WoodText" || tx.tag == "StoneText")
            {
                transform.GetComponent<RectTransform>().position += new Vector3(tx.rectTransform.position.x * 0.5f * Time.deltaTime, tx.rectTransform.position.y * 0.5f * Time.deltaTime);
            }
            else
            {
                transform.GetComponent<RectTransform>().position += new Vector3(tx.rectTransform.position.x * 0.5f * Time.deltaTime, -tx.rectTransform.position.y * 5 * Time.deltaTime);
            }
        }
    }
    private void OnDestroy()
    {
        Num.GetComponent<TextMove>().Num = FNum;
        Text PlusItem = Instantiate(Num,Cv);
        PlusItem.rectTransform.position = transform.GetComponent<RectTransform>().position;
    }
}
