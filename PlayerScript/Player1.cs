﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//동기화시 Player_Number와 Camera 설정
public class Player1 : MonoBehaviour
{
    public Camera Camera1;//멀티시 카메라 설정
    public Camera Camera2;//멀티시 카메라 설정
    public Camera Camera3;//멀티시 카메라 설정
    public Camera Camera4;//4P멀티시 카메라 설정
    public GameObject PlayerViewGun;//플레이어만 보이는 총 오브젝트
    public GameObject EnemyViewGun;//적만 보이는 총 오브젝트
    public GameObject Pick;//곡괭이 오브젝트
    public GameObject camrotation; // 카메라 로테이션 조정
    public GameObject shot; // 총알 프리팹
    //public GameObject CreatePosition;
    public GameObject Woodobj; // 나무 바닥 오브젝트 3.4 수정
    public GameObject Stoneobj; // 돌 바닥 오브젝트 3.4 수정
    public GameObject WoodWall; // 나무 벽 오브젝트 3.5 추가
    public GameObject StoneWall; // 돌 벽 오브젝트 3.5 추가
    public GameObject Buildbox; // 2.22 수정중
    public GameObject Buildbox2; // 벽 3.5 추가
    public GameObject Gas; // 독가스 충돌시 보여질 이미지
    public GameObject Hpep;//총알 맞을 시 보여질 이미지
    public Text CreateText; //오브젝트 설치 자원 텍스트
    public RawImage CreateImage; // 오브젝트 설치 자원 이미지
    public Transform shotposition; // 총알 포지션
    public GameObject Gunrotation;
    public GameObject ShotFire;
    public GameObject ShotFire2;
    public Transform FirePosition;
    public ParticleSystem PS;
    public GameObject P_position;
    public GameObject Menu;
    public AudioClip ShotSound;
    public AudioClip AttackSound;
    Slider HpBar;
    Camera cam;

    GameObject selobj; // 건설 모드 시 선택되는 오브젝트
    GameObject gb; // 건설모드 설치되는 오브젝트

    Rigidbody r_body; //리지드바디
    Animator ani; // 애니메이터
    // 슬롯 이미지
    RawImage Slot1;
    RawImage Slot2;

    //플레이어 모드 및 체력
    public int Mode = 0;
    public float hp;

    // 재료 텍스트
    Text Woodtext;
    Text Stonetext;
    Text Bullettext;
    // 재료 값
    public int Wood;
    public int Stone;

    //장전총알 및 보유 총알
    public int minBullet;
    public int maxBullet;

    int MinMaterial; // 건설에 필요한 최소 자원
    int MaxMaterial; // 건설에 필요한 보유 자원
    public int MaterialMode; // 1 = 나무 2 = 나무 3 = 철

    int CreateMode2;
    public int layerMask; // 레이 레이어마스크

    float Speed = 1.5f;
    float atk = 1.0f;
    float MouseX; // 마우스 위 아래 값
    float MouseY; // 마우스 좌 우 값
    float CreateDelay; // 건설 딜레이 및 사격 딜레이
    float AtkDleay; // 공격 모션 딜레이
    float MoveSpeed; //이동속도
    float Shotcam;
    bool JumpCheck; // 점프 가능한지 체크
    bool CreateCheck; // 바닥오브젝트 설치 체크
    bool Atk; // 공격모션 체크
    public bool b_hit;
    //플레이어 번호
    public int player = 0;
    public int Player_number = 1;
    //랭킹 관련
    public int Survive_Number;
    //살아있는거
    public int Survive = 1;

    void Start()
    {
        Player_number = 1;
        Survive = 1;
        b_hit = false;
        Shotcam = 0;
        CreateMode2 = 0;
        hp = 100;
        minBullet = 0;
        maxBullet = 10;
        selobj = Woodobj;
        JumpCheck = true;
        MinMaterial = 10;
        MaxMaterial = Wood;
        MaterialMode = 1;
        layerMask = (-1) - (1 << gameObject.layer);
        r_body = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        Slot1 = GameObject.FindGameObjectWithTag("ItemSlot1").GetComponent<RawImage>();
        Slot2 = GameObject.FindGameObjectWithTag("ItemSlot2").GetComponent<RawImage>();
        cam = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

        if (player == Player_number)
        {
            UiText(); // UI 업데이트 요소 함수
            CharacterControl(); // 캐릭터 움직임 및 행동
            sendposition();
            send_Camera();
            Change_Survive();
            Hp();
            send_resource();
            send_status();
        }
        else if (player != Player_number)
        {
            Multi_UiText();
            Mostion();
            shotHit();
            transform.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("EFValue");
            Set_Camera();//2-26
            set_position();
            Set_resource();
        }
        Select_Team();
        Mode2();
        Singleton.instance.Receive_MSG();
        receive_rayshot();
        receive_Fkey();
        receive_Zkey();
        receive_Xkey();
        receive_Vkey();
        receive_Ckey();
        receive_Rkey();
        Set_status();
        Death();
    }

    public void receive_Fkey()//2-28
    {
        if (Player_number == Singleton.instance.pFkey[Player_number].player)
        {
            set_Fkey();
            Singleton.instance.Reset_Fkey(Player_number);
        }
    }
    public void receive_Zkey()//2-28
    {
        if (Player_number == Singleton.instance.pZkey[Player_number].player)
        {
            set_Zkey();
            Singleton.instance.Reset_Zkey(Player_number);
        }
    }
    public void receive_Xkey()//2-28
    {
        if (Player_number == Singleton.instance.pXkey[Player_number].player)
        {
            set_Xkey();
            Singleton.instance.Reset_Xkey(Player_number);
        }
    }
    public void receive_Vkey()//2-28
    {
        if (Player_number == Singleton.instance.pVkey[Player_number].player)
        {
            set_Vkey();
            Singleton.instance.Reset_Vkey(Player_number);
        }
    }
    public void receive_Ckey()//2-28
    {
        if (Player_number == Singleton.instance.pCkey[Player_number].player)
        {
            set_Ckey();
            Singleton.instance.Reset_Ckey(Player_number);
        }
    }
    public void receive_Rkey()//4-08
    {
        if (Player_number == Singleton.instance.pRkey[Player_number].player)
        {
            BulletReload();
            Singleton.instance.Reset_Rkey(Player_number);
        }
    }
    public void receive_rayshot()
    {
        if (Player_number == Singleton.instance.pRayshot[Player_number].player)
        {
            Rays();
            Singleton.instance.Rayshot_Reset(Player_number);
        }
    }//update 서버
    public void sendposition()
    {
        float px = this.transform.position.x;
        float py = this.transform.position.y;
        float pz = this.transform.position.z;
        float dx = this.transform.eulerAngles.x;
        float dy = this.transform.eulerAngles.y;
        float dz = this.transform.eulerAngles.z;
        bool fMove = ani.GetBool("FMove");
        bool bMove = ani.GetBool("BMove");
        bool Run = ani.GetBool("Run");
        float sp = MoveSpeed;
        Singleton.instance.Send_PositionMSG(Player_number, px, py, pz, dx, dy, dz,sp,fMove,bMove,Run);
    }//update 서버
    public void set_position()
    {
        if (Player_number == Singleton.instance.Position[Player_number].player)
        {
            float px = Singleton.instance.Position[Player_number].px;
            float py = Singleton.instance.Position[Player_number].py;
            float pz = Singleton.instance.Position[Player_number].pz;
            float dx = Singleton.instance.Position[Player_number].dx;
            float dy = Singleton.instance.Position[Player_number].dy;
            float dz = Singleton.instance.Position[Player_number].dz;
            float sp = Singleton.instance.Position[Player_number].sp;
            bool fm = Singleton.instance.Position[Player_number].fm;
            bool bm = Singleton.instance.Position[Player_number].bm;
            bool run = Singleton.instance.Position[Player_number].run;
            ani.SetBool("FMove", fm);
            ani.SetBool("BMove", bm);
            ani.SetBool("Run", run);
            MoveSpeed = sp;
            this.transform.position = new Vector3(px, py, pz);
            this.transform.eulerAngles = new Vector3(dx, dy, dz);
        }
    }//update 서버
    public void Select_Team()
    {
        if (player == 0)
        {
            player = Singleton.instance.Team_number;
            if (player == Player_number)
            {
                Camera2.transform.GetComponent<AudioListener>().enabled = false;
                Camera3.transform.GetComponent<AudioListener>().enabled = false;
                Camera4.transform.GetComponent<AudioListener>().enabled = false;
                Camera1.enabled = true;
                Camera2.enabled = false;
                Camera3.enabled = false;
                Camera4.enabled = false;
                Woodtext = GameObject.FindGameObjectWithTag("WoodText").GetComponent<Text>();
                Stonetext = GameObject.FindGameObjectWithTag("StoneText").GetComponent<Text>();
                Bullettext = GameObject.FindGameObjectWithTag("BulletText").GetComponent<Text>();
            }
            else if (player != Player_number)
            {
                Change();
            }
        }
    }//update 서버
    public void send_rayshot()
    {
        Singleton.instance.Send_Rayshot(Player_number);
    }//controle 서버
    public void send_Camera()
    {
        float dx = camrotation.transform.eulerAngles.x;
        float dy = camrotation.transform.eulerAngles.y;
        float dz = camrotation.transform.eulerAngles.z;
        Singleton.instance.Send_CameraMSG(dx, dy, dz, Player_number);
    }//update 서버 2-26
    public void Set_Camera()
    {
        if (Player_number == Singleton.instance.pCamera[Player_number].player)
        {
            camrotation.transform.eulerAngles = new Vector3(Singleton.instance.pCamera[Player_number].cx, Singleton.instance.pCamera[Player_number].cy, Singleton.instance.pCamera[Player_number].cz);
            Singleton.instance.Reset_Camera(Player_number);
        }
    }//update 서버 2-26
    public void send_resource()
    {
        Singleton.instance.Send_Resource(Player_number, Wood, Stone);
    }
    public void Set_resource()
    {
        if (Player_number == Singleton.instance.pResource[Player_number].player)
        {
            Wood = Singleton.instance.pResource[Player_number].wood;
            Stone = Singleton.instance.pResource[Player_number].stone;
            Singleton.instance.Reset_Resource(Player_number);
        }
    }
    public void send_status()
    {
        Singleton.instance.Send_Status(Player_number, hp, minBullet, maxBullet);
    }
    public void Set_status()
    {
        if (Player_number == Singleton.instance.pStatus[Player_number].player)
        {
            hp = Singleton.instance.pStatus[Player_number].hp;
            minBullet = Singleton.instance.pStatus[Player_number].bullet_Min;
            maxBullet = Singleton.instance.pStatus[Player_number].bullet_Max;
            Singleton.instance.Reset_status(Player_number);
        }
    }
    public void Rays()
    {
        RaycastHit rayhit;
        if (Mode == 1 && minBullet > 0) //사격 모드 레이 03.04 수정중
        {
            transform.GetComponent<AudioSource>().clip = ShotSound;
            transform.GetComponent<AudioSource>().maxDistance = 100;
            transform.GetComponent<AudioSource>().Play();
            GameObject gShot = Instantiate(shot, shotposition);
            Instantiate(ShotFire, FirePosition);
            Instantiate(ShotFire2, FirePosition);
            gShot.GetComponent<Shot>().layermask = layerMask;
            shotposition.DetachChildren();
            ani.SetLayerWeight(1, 0.8f);
            minBullet--;
        }
        else if (Mode == 2)
        {
            if (MinMaterial <= MaxMaterial)
            {
                // 빌드박스 기준으로 오브젝트 설치 187 ~ 209 // 2.28 작업중
                if (Buildbox.active)
                {
                    float dis = Vector3.Distance(cam.transform.position, Buildbox.transform.position);
                    Vector3 v3 = Buildbox.transform.position - cam.transform.position;
                    if (Physics.Raycast(cam.transform.position, v3, out rayhit,dis,LayerMask.NameToLayer("UI"))) // 중복체크
                    {
                        Debug.Log(rayhit.transform.name);
                        if (rayhit.transform.tag == "floor")
                        {
                            CreateCheck = false;
                        }
                        else
                        {
                            CreateCheck = true;
                        }
                    }
                    else
                    {
                        CreateCheck = true;
                    }
                    if (Buildbox.GetComponent<buildbox>().Check && CreateCheck)
                    {
                        gb = Instantiate(selobj);
                        gb.transform.position = Buildbox.transform.position;
                        MaxMaterial -= MinMaterial;
                        MaterialChange(MaxMaterial);
                    }
                }
                else if (Buildbox2.active)
                {
                    if (Buildbox2.GetComponent<WallbuildBox>().Check)
                    {
                        gb = Instantiate(selobj);
                        gb.transform.position = Buildbox2.transform.position;
                        gb.transform.rotation = Buildbox2.transform.rotation;
                        MaxMaterial -= MinMaterial;
                        MaterialChange(MaxMaterial);
                    }
                }
            }
        }
        else if(Mode==0)
        {
            Atk = true;
            transform.GetComponent<AudioSource>().clip = AttackSound;
            transform.GetComponent<AudioSource>().maxDistance = 1;
            transform.GetComponent<AudioSource>().Play();
        }
    }
    public void UiText()
    {
        Woodtext.text = " X " + Wood;
        Stonetext.text = " X " + Stone;
        if (Mode == 1)
        {
            Bullettext.text = minBullet + " / " + maxBullet; // 장전된 총알 및 가진 총알의 갯수
        }
        else
        {
            Bullettext.text = "∞ / ∞"; // 채집무기 무한대로 표시
        }
        if (Mode == 2)
        {
            CreateModeUi();
            Bullettext.gameObject.SetActive(false); //건설 모드 시 총알 표시 텍스트 Off
            CreateText.gameObject.SetActive(true);
            CreateImage.gameObject.SetActive(true);
            Slot1.texture = Resources.Load<Texture>("floor"); // 건설모드 1번슬롯 이미지변경
            Slot2.texture = Resources.Load<Texture>("Wall"); // ""
        }
        else
        {
            Slot1.texture = Resources.Load<Texture>("pickaxe");
            Slot2.texture = Resources.Load<Texture>("kar98k");
            CreateText.gameObject.SetActive(false);
            CreateImage.gameObject.SetActive(false);
            Bullettext.gameObject.SetActive(true);
        }
        if (b_hit)
        {
            Hpep.SetActive(true);
            hp -= 10;
            b_hit = false;
        }
    }
    public void CharacterControl()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Menu.active)
            {
                Menu.SetActive(false);
            }
            else if (!Menu.active)
            {
                Menu.SetActive(true);
                Cursor.visible = true;
            }
        }
        if (!Menu.active)
        {
            Cursor.visible = false;
            CreateDelay += Time.deltaTime;
            Mostion();
            CharecterMove();
            MouseMove();
            transform.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("EFValue");
            if (Input.GetKeyDown(KeyCode.F) && CreateDelay > 0.5f) // 채집 모드 사격 모드 건설 모드 변경 키 0 == 채집 1 == 사격 2 == 건설
            {
                Singleton.instance.Send_FkeyMSG(Player_number);
                set_Fkey();//28일 수정
            }
            if (Input.GetKeyDown(KeyCode.R))// 총알 재장전
            {
                Singleton.instance.Send_RkeyMSG(Player_number);
                BulletReload();
            }
            if (Mode == 2)
            {
                if (Input.GetKeyDown(KeyCode.Z)) // 건설모드 시 바닥 오브젝트로 변경
                {
                    Singleton.instance.Send_ZkeyMSG(Player_number);
                    set_Zkey();
                }
                else if (Input.GetKeyDown(KeyCode.X)) // 건설모드 시 벽 오브젝트로 변경
                {
                    Singleton.instance.send_XkeyMSG(Player_number);
                    set_Xkey();
                    //selobj = objCreate2;
                }
                else if (Input.GetKeyDown(KeyCode.C))
                {
                    Singleton.instance.Send_CkeyMSG(Player_number);
                    set_Ckey();
                }
                if (Input.GetKeyDown(KeyCode.V)) //빌드 박스 위치 전환 
                {
                    Singleton.instance.Send_VkeyMSG(Player_number);
                    set_Vkey();
                }
                if (Buildbox.active) //3.5 수정
                {
                    //Singleton.instance.Send_Buildbox1MSG(Player_number);
                    buildbox1();
                }
            }
            if (Input.GetMouseButtonUp(0) && CreateDelay > 0.5f)
            {
                send_rayshot();
                Rays(); // 2.26일 수정
                CreateDelay = 0;
            }
            if (Input.GetKey(KeyCode.LeftShift))//달리기
            {
                MoveSpeed = 0.8f;
                ani.SetBool("Run", true);
            }
            else
            {
                MoveSpeed = 0.5f;
                ani.SetBool("Run", false);
            }
            if (Input.GetKeyDown(KeyCode.Space) && JumpCheck) //점프
            {
                r_body.AddForce(new Vector3(0, 3, 0), ForceMode.Impulse);
                JumpCheck = false;
            }
        }
    }
    private void OnTriggerStay(Collider other)//03.07
    {
        if (other.tag == "Gas") // 독가스에 맞을 시 체력 감소 및 판넬 On
        {
            hp -= 0.03f;
            if (player == Player_number)
                Gas.SetActive(true);
        }
        if (other.tag == "BulletBox" && Mode == 0 || other.tag == "wood" && Mode == 0 || other.tag == "stone" && Mode == 0)
        {
            if (Input.GetMouseButtonUp(0) && CreateDelay > 0.5f)
            {
                DIE_M dies = other.transform.GetComponent<DIE_M>();
                if (dies != null)
                {
                    ParticleSystem PCS = Instantiate(PS.gameObject).GetComponent<ParticleSystem>();
                    PCS.transform.position = new Vector3(other.transform.position.x, transform.position.y + 0.2f, other.transform.position.z);
                    PCS.transform.LookAt(transform);
                    PCS.Play();
                    dies.ondie(Player_number);
                }
            }
        }
    }
    private void OnTriggerExit(Collider other) // 독가스 탈출 시 판넬 Off
    {
        Gas.SetActive(false);
    }
    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint conp = collision.contacts[0];
        if (conp.normal.normalized.y > 0.5) // 3.18 추가 점프 체크
        {
            JumpCheck = true;
        }
    }

    float fCal(float fx, int ix)// 설치 오브젝트 위치 조정
    {
        float r = ix + fx;
        if (Mathf.Abs(fx) > 0.25f && Mathf.Abs(fx) < 0.5f)
            fx = 0.5f;
        else if (Mathf.Abs(fx) < 0.25f && Mathf.Abs(fx) > 0)
            fx = 0;
        else if (Mathf.Abs(fx) > 0.75f && Mathf.Abs(fx) < 1)
            fx = 1f;
        else if (Mathf.Abs(fx) < 0.75f && Mathf.Abs(fx) > 0.5f)
            fx = 0.5f;
        float rx = Mathf.Abs(ix) + fx;
        if (r < 0)
            rx = -rx;
        return rx;
    }
    float fCal2(float fx, int ix) //설치 오브젝트 높 낮이 조정
    {
        fx = Mathf.Round(fx * 10) * 0.1f;
        if (CreateMode2 == 2)
        {
            fx -= 0.3f;
        }
        else
        {
            fx -= 0.1f;
        }
        float rx = ix + fx;
        return rx;
    }
    /*float fCal3(float fx, int ix) // 벽 설치 오브젝트 높 낮이 조정  * 3.5  추가
    {
        fx = Mathf.Round(fx * 10) * 0.1f;
        if (Fb)
        {
            fx -= 0.1f;
        }
        float rx = ix + fx;
        return rx;
    }
    float fCal4(float fx, int ix) // 벽 설치 오브젝트 위치 조정 *3.7 수정
    {
        float r = ix + fx;
        if (Mathf.Abs(fx) > 0.25f && Mathf.Abs(fx) < 0.5f)
            fx = 0.5f;
        else if (Mathf.Abs(fx) < 0.25f && Mathf.Abs(fx) > 0)
            fx = 0;
        else if (Mathf.Abs(fx) > 0.75f && Mathf.Abs(fx) < 1)
            fx = 0.5f;
        else if (Mathf.Abs(fx) < 0.75f && Mathf.Abs(fx) > 0.5f)
            fx = 0.5f;
        float rx = Mathf.Abs(ix) + fx;
        if (r < 0)
            rx = -rx;
        return rx;
    }*/
    void buildbox1() //2.28 수정중
    {
        Buildbox.transform.position = transform.position + transform.forward * 0.3f;
        Buildbox.transform.localPosition += new Vector3(0, 0.15f, 0);
        int ix = (int)Buildbox.transform.position.x;
        float fx = Buildbox.transform.position.x % 1f;
        float rx = fCal(fx, ix);
        int iy = (int)Buildbox.transform.position.y;
        float fy = Buildbox.transform.position.y % 1f;
        float ry = fCal2(fy, iy);
        int iz = (int)Buildbox.transform.position.z;
        float fz = Buildbox.transform.position.z % 1f;
        float rz = fCal(fz, iz);
        Buildbox.transform.position = new Vector3(rx, ry, rz);
        buildbox bx = Buildbox.GetComponent<buildbox>();
        bx.Rays();
    }
    /*void bulidbox2() // *  3.5 추가 
    {
        Buildbox2.transform.position = transform.position + transform.forward * 0.4f;
        Buildbox2.transform.localPosition += new Vector3(0, 0.2f, 0);
        int ix = (int)Buildbox2.transform.position.x;
        float fx = Buildbox2.transform.position.x % 1f;
        float rx = fCal4(fx, ix);
        int iy = (int)Buildbox2.transform.position.y;
        float fy = Buildbox2.transform.position.y % 1f;
        float ry = fCal3(fy, iy);
        int iz = (int)Buildbox2.transform.position.z;
        float fz = Buildbox2.transform.position.z % 1f;
        float rz = fCal4(fz, iz);
        if (transform.eulerAngles.y > 0 && transform.eulerAngles.y < 50) // 벽 회전 값 조정
        {
            Buildbox2.transform.localRotation = Quaternion.Euler(0, 90, 0);
        }
        else if (transform.eulerAngles.y > 50 && transform.eulerAngles.y < 150)
        {
            Buildbox2.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (transform.eulerAngles.y > 150 && transform.eulerAngles.y < 210)
        {
            Buildbox2.transform.localRotation = Quaternion.Euler(0, 90, 0);
        }
        else if (transform.eulerAngles.y > 210 && transform.eulerAngles.y < 310)
        {
            Buildbox2.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        if (Buildbox2.transform.rotation.y > 0) // 벽 빌드박스 z 값 조정
        {
            rz -= 0.25f;
        }
        else // 벽 빌드박스 x 값 조정
        {
            rx -= 0.25f;
        }
        Buildbox2.transform.position = new Vector3(rx, ry, rz);
    }*/
    void CreateModeUi() // 건설 모드 ui 이미지 및 텍스트 교체  * 3.5 수정 
    {
        if (MaterialMode == 1)
        {
            if (Buildbox.active)
            {
                selobj = Woodobj;
            }
            else if (Buildbox2.active)
            {
                selobj = WoodWall;
            }
            MaxMaterial = Wood;
            CreateImage.texture = Resources.Load<Texture>("wood_tex");
        }
        else if (MaterialMode == 2)
        {
            if (Buildbox.active)
            {
                selobj = Stoneobj;
            }
            else if (Buildbox2.active)
            {
                selobj = StoneWall;
            }
            MaxMaterial = Stone;
            CreateImage.texture = Resources.Load<Texture>("stone_tex");
        }
        CreateText.text = MaxMaterial + " / " + MinMaterial;
    }
    void MaterialChange(int MaxMaterial)
    {
        if (MaterialMode == 1)
        {
            Wood = MaxMaterial;
        }
        else if (MaterialMode == 2)
        {
            Stone = MaxMaterial;
        }
    }
    public void set_Fkey()
    {
        if (Mode == 0)
        {
            Shotcam = -20;
            Mode = 1;
        }
        else if (Mode == 1)
        {
            Mode = 2;
            Buildbox.SetActive(true);
            Shotcam = 0;
        }
        else
        {
            Mode = 0;
            Buildbox.SetActive(false);//
            Buildbox2.SetActive(false);
            Shotcam = 0;
        }
    }//2-28
    public void set_Zkey()
    {
        Buildbox.SetActive(true);
        Buildbox2.SetActive(false);
        selobj = Woodobj; // 3.04 수정
    }
    public void set_Xkey()
    {
        Buildbox2.SetActive(true);
        Buildbox.SetActive(false);
        selobj = WoodWall;
    }
    public void set_Ckey()
    {
        MaterialMode++;
        if (MaterialMode == 4)
        {
            MaterialMode = 1;
        }
    }
    public void set_Vkey()
    {
        CreateMode2++;
        if (CreateMode2 == 3)
        {
            CreateMode2 = 1;
        }
    }
    public void Multi_UiText()
    {
        if (Mode == 2)
        {
            CreateModeUi();
        }

    }
    public void Mode2()
    {
        if (Mode == 2)
        {
            if (Buildbox.active) //3.5 수정
            {
                buildbox1();
            }
        }
    }
    public void Death()
    {
        if (Singleton.instance.Survive_Number == 1)
        {
            Cursor.visible = true;
            Singleton.instance.Set_win();
            if (player == Player_number)
                SceneManager.LoadScene("GameOver");
        }
        if (hp <= 0&&Survive==1)
        {
            Survive = 0;
            Singleton.instance.Set_rank();
            StartCoroutine("Destroy");
        }
    }
    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(0.1f);
        if (player == Player_number)
            SceneManager.LoadScene("GameOver");
        Destroy(gameObject);
    }
    void CharecterMove() // 캐릭터 이동 및 이동시 애니메이션
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
            Vector3 position = new Vector3(x, 0, z);
            transform.position += ((P_position.transform.rotation * position) * MoveSpeed) * Time.deltaTime;
            if (z < 0)
            {
                if (MoveSpeed > 0.5f)
                {
                    ani.SetBool("Run", true);
                }
                else
                {
                    ani.SetBool("BMove", true);
                }
            }
            else if (Mathf.Abs(x) > 0 || z > 0)
            {
                if (MoveSpeed > 0.5f)
                {
                    ani.SetBool("Run", true);
                }
                else
                {
                    ani.SetBool("FMove", true);
                }
            }
            else
            {
                ani.SetBool("FMove", false);
                ani.SetBool("BMove", false);
                ani.SetBool("Run", false);
            }
    }
    void MouseMove()
    {
        MouseY += Speed * Input.GetAxis("Mouse X");
        MouseX += Speed * Input.GetAxis("Mouse Y");
        MouseX = Mathf.Clamp(MouseX, -60f, 60f); // 마우스 최소값 최대값 설정
        this.transform.eulerAngles = new Vector3(0, MouseY, 0);
        camrotation.transform.eulerAngles = new Vector3(-MouseX, Shotcam + MouseY, 0); // 마우스 위 아래 좌표 따라 변경
        P_position.transform.localRotation = Quaternion.Euler(0, Shotcam, 0);

    }
    void Mostion() // 3.19 추가 캐릭터 애니메이션
    {
        if (Mode == 0)
        {
            Pick.SetActive(true);
            ani.SetBool("ATK", Atk);
            ani.SetLayerWeight(1,0.5f);
            if (Atk)
            {
                AtkDleay += Time.deltaTime;
                if (AtkDleay > 0.5f)
                {
                    AtkDleay = 0;
                    Atk = false;
                }
            }
        }
        else
        {
            Pick.SetActive(false);
        }
        if (Mode == 1)
        {
            ani.SetBool("Shot", true);
            if (transform.gameObject.layer == 9)
            {
                PlayerViewGun.SetActive(true);
            }
            else
            {
                EnemyViewGun.SetActive(true);
            }
            ani.SetLayerWeight(1, 1);
            if (Input.GetMouseButtonDown(1) && cam.fieldOfView >= 60)
            {
                cam.fieldOfView = 10;
            }
            else if (Input.GetMouseButtonDown(1) && cam.fieldOfView <= 10)
            {
                cam.fieldOfView = 60;
            }
        }
        else
        {
            ani.SetBool("Shot", false);
            if (transform.gameObject.layer == 9)
            {
                PlayerViewGun.SetActive(false);
            }
            else
            {
                EnemyViewGun.SetActive(false);
            }
        }
    }
    void Change()
    {
        Transform[] tb = gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform tbs in tb)
        {
            if (tbs.gameObject.layer != 5 && tbs.gameObject.layer !=12 &&tbs.gameObject.layer !=13)
            {
                tbs.gameObject.layer = 10;
            }
        }
        layerMask = (-1) - (1 << gameObject.layer);
    }//레이어 체인지
    void BulletReload() // 총알 재장전
    {
        if (maxBullet > 0)
        {
            for (int i = minBullet; i < 5;)
            {
                if (maxBullet <= 0)
                {
                    break;
                }
                i++;
                minBullet++;
                maxBullet--;
            }
        }
    }
    void Change_Survive()
    {
        if (Survive_Number != Singleton.instance.Survive_Number)
            Survive_Number = Singleton.instance.Survive_Number;
    }
    void Hp()
    {
        HpBar = GameObject.FindGameObjectWithTag("HP").GetComponent<Slider>();
        HpBar.value = hp;
    }
    void shotHit()
    {
        if (b_hit)
        {
            hp -= 10;
            b_hit = false;
        }
    }
}
