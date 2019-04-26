using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Script : MonoBehaviour
{
    Rigidbody rigidbody;
    Animator ani;
    public float x_sensitivity = 1.0f;
    public float y_sensitivity = 1.0f;
    public int Player_number = 1;
    public int player=0;
    // Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Singleton.instance.Receive_MSG();
        Select_Team();
        PlayerControle();
        sendposition();
        set_position();

    }

    public void Select_Team()
    {
        if (player == 0)
        {
            player = Singleton.instance.Team_number;
        }
    }
    public void PlayerControle()
    {
        if (player == 1)//플레이어 번호 (1이면 1p, 2면 2p
        {
            int layerMask = 9;
            float MouseY = Input.GetAxis("Mouse X") * x_sensitivity;
            float MouseX = Input.GetAxis("Mouse Y") * y_sensitivity;

            this.transform.localRotation *= Quaternion.Euler(0, MouseY, 0);
            
 
            
            if (Input.GetKey(KeyCode.W))
            {
                this.transform.position = this.transform.position + this.transform.forward * 0.05f;

            }
            else if (Input.GetKey(KeyCode.S))
            {
                this.transform.position = this.transform.position - this.transform.forward * 0.05f;
            }
            if (Input.GetKey(KeyCode.A))
            {
                this.transform.position = this.transform.position - this.transform.right * 0.05f;
            }
            if (Input.GetKey(KeyCode.D))
            {
                this.transform.position = this.transform.position + this.transform.right * 0.05f;
            }
            if (Input.GetKey(KeyCode.Space))
            {
                rigidbody.velocity = new Vector3(0, 5f, 0);
            }
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit rayhit;
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out rayhit, 3.5f, layerMask))
                {
                    if (rayhit.transform.tag == "item")
                    {
                        DIE dies = rayhit.transform.GetComponent<DIE>();
                        dies.ondie();
                    }
                }
            }
        }
    }
    public void sendposition()
    {
        {
            float px = this.transform.position.x;
            float py = this.transform.position.y;
            float pz = this.transform.position.z;
            float dx = this.transform.eulerAngles.x;
            float dy = this.transform.eulerAngles.y;
            float dz = this.transform.eulerAngles.z;
            Singleton.instance.Send_PositionMSG(Player_number, px, py, pz, dx, dy, dz);
        }
    }
    public void set_position()
    {
        if (Player_number == Singleton.instance.Position.player)
        {
            float px = Singleton.instance.Position.px;
            float py = Singleton.instance.Position.py;
            float pz = Singleton.instance.Position.pz;
            float dx = Singleton.instance.Position.dx;
            float dy = Singleton.instance.Position.dy;
            float dz = Singleton.instance.Position.dz;
            this.transform.position = new Vector3(px, py, pz);
            this.transform.eulerAngles = new Vector3(dx, dy, dz);
        }

    }
    public void send_rayshot()
    {
    }
}
