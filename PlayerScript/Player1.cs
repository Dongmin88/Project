using System.Collections;
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
    public GameObject Shotep1;//총구화염 이펙트
    public GameObject Shotep2;//""
    public Transform T_Shotep1;//총구화염 이펙트 출력 위치
    public Transform T_Shotep2;//""
    public ParticleSystem PS;//채집 모드 오브젝트 타격 시 생성 파티클
    public ParticleSystem Blood;//총알 피격 시 생성 파티클
    public GameObject P_position;//기준으로 이동
    public GameObject Menu; // 옵션창
    public GameObject ItemWindow;//총기류 상자 획득시 출력될 오브젝트
    public GameObject AimPointer; // 사격모드 시 포인터
    public GameObject Pointer; // ""
    public AudioClip AttackSound;//어택 시 사운드
    public GameObject MaterialImage;
    public RectTransform Canvas;
    Slider HpBar; // Hp바
    Camera cam;//자신의 카메라

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
    public int GunNum; // 총기 넘버
    public string Guns; // 총기 이름
    int MinMaterial; // 건설에 필요한 최소 자원
    int MaxMaterial; // 건설에 필요한 보유 자원
    public int MaterialMode; // 1 = 나무 2 = 나무 3 = 철
    int CreateMode2; // 추후 설정
    //int CreateMode2;
    public int layerMask; // 레이 레이어마스크
    float Speed = 1.5f; //마우스 스피드
    float MouseX; // 마우스 위 아래 값
    float MouseY; // 마우스 좌 우 값
    float CreateDelay; // 건설 딜레이 및 사격 딜레이
    float AtkDleay; // 공격 모션 딜레이
    float MoveSpeed; //이동속도
    float Shotcam;
    float ShotDelay;//총기사격 딜레이
    public float GasDam; // 가스데미지
    public float GasDamDelay;//가스 피격 시간 설정
    bool JumpCheck; // 점프 가능한지 체크
    bool CreateCheck; // 바닥오브젝트 설치 체크
    bool Atk; // 공격모션 체크
    public bool b_hit; // 총알 피격 유무 설정
    //플레이어 번호
    public int player = 0;
    public int Player_number;
    //랭킹 관련
    public int Survive_Number;
    //살아있는거
    public int Survive = 1;

    //죽었을때 박스에 자원 넣기
    public GameObject Playerbox;

    void Start()
    {
        Guns = "";
        GunNum = 0;
        GasDam = 0.5f;
        Player_number = 1;
        Survive = 1;
        b_hit = false;
        Shotcam = 0;
        hp = 100;
        minBullet = 0;
        maxBullet = 20;
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
        Select_Team();
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
            PlayerGunChange();
            ShotPointer();
        }
        else if (player != Player_number)
        {
            Multi_UiText();
            Mostion();
            shotHit();
            transform.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("EFValue");
            set_position();
            Set_Camera();//2-26
            Set_resource();
            receive_rayshot();
            receive_Fkey();
            receive_Zkey();
            receive_Xkey();
            receive_Vkey();
            receive_Ckey();
            receive_Rkey();
            Set_status();
            EnemyGunChange();
            logout_player();
        }
        Mode2();
        Singleton.instance.Receive_MSG();
        Death();
    }
    //서버에서 받아온 적 플레이어 정보 갱신
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
        /*if (Player_number == Singleton.instance.pVkey[Player_number].player)
        {
            set_Vkey();
            Singleton.instance.Reset_Vkey(Player_number);
        }*/
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
        bool Atk = ani.GetBool("ATK");
        float sp = MoveSpeed;
        Singleton.instance.Send_PositionMSG(Player_number, px, py, pz, dx, dy, dz,sp,fMove,bMove,Run,Atk);
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
            bool atk = Singleton.instance.Position[Player_number].atk;
            ani.SetBool("FMove", fm);
            ani.SetBool("BMove", bm);
            ani.SetBool("Run", run);
            ani.SetBool("ATK", atk);
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
        Singleton.instance.Send_Status(Player_number, hp, minBullet, maxBullet, Mode, CreateMode2, MaterialMode,GunNum);
    }
    public void Set_status()
    {
        if (Player_number == Singleton.instance.pStatus[Player_number].player)
        {
            hp = Singleton.instance.pStatus[Player_number].hp;
            minBullet = Singleton.instance.pStatus[Player_number].bullet_Min;
            maxBullet = Singleton.instance.pStatus[Player_number].bullet_Max;
            Mode = Singleton.instance.pStatus[Player_number].Mode;
            CreateMode2 = Singleton.instance.pStatus[Player_number].CreateMode2;
            MaterialMode = Singleton.instance.pStatus[Player_number].MaterialMode;
            GunNum = Singleton.instance.pStatus[Player_number].GunNum;
            Singleton.instance.Reset_status(Player_number);
        }
    }
    public void Rays()
    {
        RaycastHit rayhit;
        if (Mode ==0)
        {
            Atk = true;
            transform.GetComponent<AudioSource>().clip = AttackSound;
            transform.GetComponent<AudioSource>().Play();
            if (player == Player_number)
            {
                if (Physics.Raycast(cam.transform.position, cam.transform.forward, out rayhit, 0.2f, layerMask))
                {
                    DIE_M dies = rayhit.transform.GetComponent<DIE_M>();
                    if (dies != null)
                    {
                        ParticleSystem PCS = Instantiate(PS.gameObject).GetComponent<ParticleSystem>();
                        PCS.transform.position = new Vector3(rayhit.transform.position.x, transform.position.y + 0.2f, rayhit.transform.position.z);
                        PCS.transform.LookAt(transform);
                        PCS.Play();
                        dies.ondie(Player_number);
                    }
                }
            }
        }
        else if (Mode == 1 && minBullet > 0) //사격 모드 레이 03.04 수정중
        {
            if (player == Player_number&&cam.fieldOfView > 10)
            {
                GameObject Sep1 = Instantiate(Shotep1, T_Shotep1);
                GameObject Sep2 = Instantiate(Shotep2, T_Shotep1);
            }
            else
            {
                GameObject Sep1 = Instantiate(Shotep1, T_Shotep2);
                GameObject Sep2 = Instantiate(Shotep2, T_Shotep2);
            }
            GameObject gShot = Instantiate(shot, shotposition);
            gShot.GetComponent<Shot>().name = Guns;
            gShot.GetComponent<Shot>().layermask = layerMask;
            MouseX += G_Rebound(Guns, 2);
            MouseY += G_Rebound(Guns, 1);
            shotposition.DetachChildren();
            ani.SetLayerWeight(1, 0.8f);
            minBullet--;
        }
        else if (Mode == 2) // 빌트박스를 통해 건물 오브젝트 설치
        {
            if (MinMaterial <= MaxMaterial)
            {
                if (Buildbox.active)
                {
                    float dis = Vector3.Distance(cam.transform.position, Buildbox.transform.position);
                    Vector3 v3 = Buildbox.transform.position - cam.transform.position;
                    if (Physics.Raycast(cam.transform.position, v3, out rayhit,dis,LayerMask.NameToLayer("UI"))) // 중복체크
                    {
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
                        MaxMaterial -= MinMaterial;
                        MaterialChange(MaxMaterial);
                        Singleton.instance.Send_Floor(MaterialMode, Buildbox.transform.position.x, Buildbox.transform.position.y, Buildbox.transform.position.z);
                        gb = Instantiate(selobj);
                        gb.transform.position = Buildbox.transform.position;
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
    }
    public void UiText() // Player의 자원 이미지 및 체력바 && 선택된 Mode 이미지 출력
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
            b_hit = false;
        }
    }
    public void CharacterControl() // 캐릭터 컨트롤러 
    {
        CreateDelay += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Escape)) // 게임 도중 옵션창을 불러냅니다. 
        {
            if (Menu.active)
            {
                Menu.SetActive(false);
            }
            else if (!Menu.active)
            {
                if (ItemWindow.active)
                {
                    ItemWindow.SetActive(false);
                }
                else
                {
                    Menu.SetActive(true);
                    Cursor.visible = true;
                }
            }
        }
        if (ItemWindow.active) // 아이템상자 획득 시 마우스커서 On
        {
            Cursor.visible = true;
        }
        if (!Menu.active&&!ItemWindow.active&&hp>0) // 옵셩창과 아이템상자창이 Active 되어있지 않고 Hp가 0보다 높아야지만 캐릭터 컨트롤 가능
        {
            Cursor.visible = false;
            Mostion();
            CharecterMove();
            MouseMove();
            transform.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("EFValue");
            if (Input.GetKeyDown(KeyCode.F) && CreateDelay > 0.5f) // 채집 모드 사격 모드 건설 모드 변경 키 0 == 채집 1 == 사격 2 == 건설
            {
                Singleton.instance.Send_FkeyMSG(Player_number);
                set_Fkey();//28일 수정
            }
            if (Input.GetKeyDown(KeyCode.R) && Mode==1)// 총알 재장전
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
                /*else if (Input.GetKeyDown(KeyCode.X)) // 건설모드 시 벽 오브젝트로 변경
                {
                    Singleton.instance.send_XkeyMSG(Player_number);
                    set_Xkey();
                    //selobj = objCreate2;
                }*/
                else if (Input.GetKeyDown(KeyCode.C)) // 건설 오브젝트 Type 바꾸기 Wood or Stone
                {
                    Singleton.instance.Send_CkeyMSG(Player_number);
                    set_Ckey();
                }
                /*if (Input.GetKeyDown(KeyCode.V)) //빌드 박스 위치 전환 
                {
                    Singleton.instance.Send_VkeyMSG(Player_number);
                    set_Vkey();
                }*/
                if (Buildbox.active) //3.5 수정
                {
                    //Singleton.instance.Send_Buildbox1MSG(Player_number);
                    buildbox1();
                }
            }
            if (Input.GetMouseButton(0) && CreateDelay > ShotDelay && Mode ==1) // 사격 모드 일때 마우스 버튼 클릭 시 함수 작동
            {
                send_rayshot();
                Rays();
                CreateDelay = 0;
            }
            else if (CreateDelay > ShotDelay) // 총을 쏘는 동안 마우스의 이동속도를 제한한 것을 풀어줌.
            {
                Speed = 1.5f;
            }
            if(Input.GetMouseButtonUp(0)&& Mode == 0 &&!Atk)// 채집모드일때 마우스 버튼 클릭 시 사운드 교체 및 애니메이션 교체
            {
                transform.GetComponent<AudioSource>().clip = AttackSound;
                transform.GetComponent<AudioSource>().Play();
                Atk = true;
                send_rayshot();
                Rays();
            }
            if (Input.GetMouseButtonUp(0) &&CreateDelay >0.3f && Mode == 2) // 건설 모드일때 마우스 버튼 클릭 시 함수 작동
            {
                send_rayshot();
                Rays();
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
            if (player == Player_number)
            {
                Gas.SetActive(true);
                GasDamDelay += Time.deltaTime;
                if (GasDamDelay > 1)
                {
                    hp -= GasDam;
                    GasDamDelay = 0;
                }
            }
        }
        if (player == Player_number)
        {
            if (other.tag == "PlayerBox") // 적 플레이어 사망후 남는 시체상자를 채집
            {
                if (Input.GetMouseButtonUp(0) && Mode == 0)
                {
                    Atk = true;
                    transform.GetComponent<AudioSource>().clip = AttackSound;
                    transform.GetComponent<AudioSource>().Play();
                    PlayerBox PB = other.transform.GetComponent<PlayerBox>();
                    if (PB != null)
                    {
                        ParticleSystem PCS = Instantiate(PS.gameObject).GetComponent<ParticleSystem>();
                        PCS.transform.position = new Vector3(other.transform.position.x, transform.position.y + 0.2f, other.transform.position.z);
                        PCS.transform.LookAt(transform);
                        PCS.Play();
                        PB.ondie(Player_number);
                    }
                    CreateDelay = 0;
                }
            }
            if (other.tag == "SupplyBox") // 게임 도중 떨어지는 보급상자를 채집
            {
                if (Input.GetMouseButtonUp(0) && Mode == 0)
                {
                    Atk = true;
                    transform.GetComponent<AudioSource>().clip = AttackSound;
                    transform.GetComponent<AudioSource>().Play();
                    SupplyBox SB = other.GetComponent<SupplyBox>();
                    if (SB != null)
                    {
                        ParticleSystem PCS = Instantiate(PS.gameObject).GetComponent<ParticleSystem>();
                        PCS.transform.position = new Vector3(other.transform.position.x, transform.position.y + 0.2f, other.transform.position.z);
                        PCS.transform.LookAt(transform);
                        PCS.Play();
                        SB.onDemage(Player_number);
                    }
                    CreateDelay = 0;
                }
            }
        }
    }
    private void OnTriggerExit(Collider other) // 독가스 탈출 시 판넬 Off
    {
        Gas.SetActive(false);
        GasDamDelay = 0;
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
        if (MouseX < -20)
        {
            fx -= 0.5f;
        }
        else
        {
            fx -= 0.2f;
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
    void buildbox1() // 빌드박스 위치 설정.
    {
        Buildbox.transform.position = transform.position + transform.forward * 0.4f;
        Buildbox.transform.localPosition += new Vector3(0, 0.35f, 0);
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
    void MaterialChange(int MaxMaterial) // 건물 오브젝트 설치 후 자원 감소
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
    public void set_Fkey() // 플레이어의 모드 설정 0 == 채집 1 == 사격 2 == 건설
    {
        if (Mode == 0)// Mode가 0일때 F키를 누르게 되면 들어온다.
        {
            if (player == Player_number) //자기자신이 플레이하는 캐릭터 일 경우 PlayerViewGun이 켜지고 아니면 EnemyViewGun이 켜진다.
            {
                PlayerViewGun.SetActive(true); // 나의 총기 표시
            }
            else
            {
                EnemyViewGun.SetActive(true); // 적군의 총기 표시.
            }
            EnemyViewGun.GetComponent<GunView>().ViewGun(GunNum); // 총기 정보를 넘겨줌
            PlayerViewGun.GetComponent<GunView>().ViewGun(GunNum); // ""
            if (Guns.Equals("AK-74m")) //총기 종류에 따라 발사간격을 다르게 줌.
            {
                ShotDelay = 0.15f;
            }
            else if (Guns.Equals("M4A1_PBR"))
            {
                ShotDelay = 0.1f;
            }
            else if (Guns.Equals("RevolverM1879"))
            {
                ShotDelay = 0.8f;
            }
            Shotcam = -20;
            MouseY += 20;
            Mode = 1;
            if (Guns.ToString() == "") // 총기가 없을 경우 건설모드로 넘겨진다.
            {
                Mode = 2;
                Buildbox.SetActive(true);
            }
        }
        else if (Mode == 1) // 사격모드 일때 F키를 누르면 들어오게 된다.
        {
            Mode = 2;
            Buildbox.SetActive(true); // 건설에 필요한 빌드박스를 화면에 출력
            MouseY -= 20;
            Shotcam = 0;
            if (player == Player_number) // 사격모드 일때 출력된 총기류를 꺼준다.
            {
                PlayerViewGun.SetActive(false);
            }
            else
            {
                EnemyViewGun.SetActive(false);
            }
        }
        else // 채집모드
        {
            if (player == Player_number) 
            {
                PlayerViewGun.SetActive(false);
            }
            else
            {
                EnemyViewGun.SetActive(false);
            }
            Mode = 0;
            Buildbox.SetActive(false);//
            Buildbox2.SetActive(false);
            Shotcam = 0;
        }
    }//2-28
    public void set_Zkey() //바닥 오브젝트를 선택
    {
        Buildbox.SetActive(true);
        Buildbox2.SetActive(false);
        selobj = Woodobj;
    }
    public void set_Xkey() // 벽 오브젝트 선택
    {
        Buildbox2.SetActive(true);
        Buildbox.SetActive(false);
        selobj = WoodWall;
    }
    public void set_Ckey() // 건물 오브젝트 Type 변경
    {
        MaterialMode++;
        if (MaterialMode == 3)
        {
            MaterialMode = 1;
        }
    }
    /*public void set_Vkey()
    {
        CreateMode2++;
        if (CreateMode2 == 3)
        {
            CreateMode2 = 1;
        }
    }*/
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
    public void Death() //플레이어 사망 체크
    {
        if (Singleton.instance.Survive_Number == 1 && Survive == 1)
        {
            Cursor.visible = true;
            Singleton.instance.Set_win();
            StartCoroutine("victory");
        }
        if (hp <= 0&&Survive==1)
        {
            ani.SetFloat("Hp", hp);
            Survive = 0;
            StartCoroutine("Destroy");
        }
    }
    IEnumerator victory()//승리시 들어오게 되는 코루틴
    {
        yield return new WaitForSeconds(3);

        if (player == Player_number)
            SceneManager.LoadScene("GameOver");
    }
    IEnumerator Destroy() //플레이어 사망 시 들어오게 되는 코루틴
    {
        yield return new WaitForSeconds(2);
        Singleton.instance.Set_rank();
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
    void MouseMove() //마우스 위치를 받아와 캐릭터의 Rotation을 바꾸어 준다.
    {
        MouseY += Speed * Input.GetAxis("Mouse X");
        MouseX += Speed * Input.GetAxis("Mouse Y");
        MouseX = Mathf.Clamp(MouseX, -60f, 80f); // 마우스 최소값 최대값 설정
        this.transform.eulerAngles = new Vector3(0,MouseY, 0);
        camrotation.transform.eulerAngles = new Vector3(-MouseX, Shotcam + MouseY, 0); // 마우스 위 아래 좌표 따라 변경
        P_position.transform.localRotation = Quaternion.Euler(0, Shotcam, 0);

    }
    void Mostion() // 3.19 추가 캐릭터 애니메이션
    {
        if (Mode == 0)
        {
            cam.fieldOfView = 60;
            Pick.SetActive(true);
            ani.SetLayerWeight(1, 0.5f);
            if (Atk)
            {
                ani.SetBool("ATK", Atk);
                ani.SetLayerWeight(1,0.8f);
                AtkDleay += Time.deltaTime;
                if (AtkDleay > 0.5f)
                {
                    AtkDleay = 0;
                    Atk = false;
                    ani.SetBool("ATK", Atk);
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
        else if(Mode==2)
        {
            cam.fieldOfView = 60;
            ani.SetLayerWeight(1, 0.5f);
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
    void Change() //게임 시작시 설정된 Layer를 바꾸어준다.
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
    }
    void BulletReload() // 총알 재장전
    {
        int Reload;
        if (Guns.Equals("AK-74m") || Guns.Equals("M4A1_PBR"))
        {
            Reload = 30;
        }
        else
        {
            Reload = 5;
        }
        if (maxBullet > 0)
        {
            for (int i = minBullet; i < Reload;)
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
    void Hp() // HpBar 설정
    {
        HpBar = GameObject.FindGameObjectWithTag("HP").GetComponent<Slider>();
        HpBar.value = hp;
    }
    void shotHit() //총알에 맞았을 경우 작동
    {
        if (b_hit)
        {
            ParticleSystem ps = Instantiate(Blood.gameObject, transform).GetComponent<ParticleSystem>();
            ps.Play();
            b_hit = false;
        }
    }
    float G_Rebound(string GunName, int Type) //총기류 반동 설정
    {
        float rebound = 0;
        if (Type == 1)
        {
            if (GunName.Equals("AK-74m"))
            {
                float[] ft = { 0, -0.5f, 0.5f };
                rebound = ft[Random.Range(1, 3)];
            }
            else if (GunName.Equals("M4A1_PBR"))
            {
                float[] ft = { 0, -0.4f, 0.4f };
                rebound = ft[Random.Range(1, 3)];
            }
            else if (GunName.Equals("RevolverM1879"))
            {
                rebound = 0;
            }
        }
        else if (Type == 2)
        {
            if (GunName.Equals("AK-74m"))
            {
                rebound = 1f;
                Speed = 0.2f;
            }
            else if (GunName.Equals("M4A1_PBR"))
            {
                rebound = 0.8f;
                Speed = 0.2f;
            }
            else if (GunName.Equals("RevolverM1879"))
            {
                rebound = 5;
                Speed = 1.5f;
            }
        }
        return rebound;
    }
    void PlayerGunChange() // 획득한 총기를 Int형으로 바꾸어 서버로 넘겨줌
    {
        if (Guns.Equals("AK-74m"))
        {
            GunNum = 1;
        }
        else if (Guns.Equals("M4A1_PBR"))
        {
            GunNum = 2;
        }
        else if (Guns.Equals("RevolverM1879"))
        {
            GunNum = 3;
        }
    }
    void EnemyGunChange() // 서버에서 받은 Int형 값으로 총기류 설정
    {
        if (GunNum == 1)
        {
            Guns = "AK-74m";
        }
        else if (GunNum == 2)
        {
            Guns = "M4A1_PBR";
        }
        else if (GunNum == 3)
        {
            Guns = "RevolverM1879";
        }
    }
    private void OnDestroy() //플레이어가 죽고 시체상자를 남김
    {
        GameObject PBox = Instantiate(Playerbox);
        PBox.transform.position = this.transform.position;
        PBox.GetComponent<PlayerBox>().Wood = Wood;
        PBox.GetComponent<PlayerBox>().Stone = Stone;
        PBox.GetComponent<PlayerBox>().Bullet = maxBullet;
    }
    public void GetMaterial(float Material, string Name)// 보급상자 획득 재료 이미지 출력
    {
        MaterialImage.name = Name;
        MaterialImage.transform.GetComponent<ImageMove>().Cv = Canvas;
        MaterialImage.transform.GetComponent<ImageMove>().FNum = Material;
        RawImage Ri = Instantiate(MaterialImage.GetComponent<RawImage>(), Canvas);
    }
    void ShotPointer()
    {
        if (Mode == 1)
        {
            if (cam.fieldOfView < 60)
            {
                AimPointer.SetActive(true);
                Pointer.SetActive(false);
            }
            else if(cam.fieldOfView > 10)
            {
                AimPointer.SetActive(false);
                Pointer.SetActive(true);
            }
        }
        else
        {
            AimPointer.SetActive(false);
            Pointer.SetActive(false);
        }
    }
    void logout_player()
    {
        if (Player_number == Singleton.instance.logout.player)
        {
            hp = 0;
            Singleton.instance.Reset_logout();
        }
    }
}
