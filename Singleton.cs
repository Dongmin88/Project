using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

enum eMSG
{
    em_USER_LOGIN = 1,
    em_USER_MATCHING,
    em_USER_TEAM,
    em_USER_POSITION,
    em_USER_MESSAGE,
    em_USER_RAYSHOT,
    em_USER_CAMERA,
    em_USER_FKEY,
    em_USER_ZKEY,
    em_USER_XKEY,
    em_USER_CKEY,
    em_USER_VKEY,
    em_USER_RKEY,
    em_USER_GAMEDEATH,
    em_USER_RESOURCE,
    em_USER_STATUS,
    em_OBJECT_TREE,
    em_OBJECT_STONE,
    em_OBJECT_BULLET,
    em_MAP_MAPSETTING,
    em_OBJECT_FLOOR,
    em_OBJECT_SUPPLY,
    em_USER_LOGOUT,
};
public struct sFlag//flag
{
    public char flag;
}
public struct sLogin//login
{
    public char flag;
    public int idx;
    public int sock;
    public int status;//0로그아웃, 1 로그인, 2, 중복
}
public struct sMap//Map
{
    public char flag;
    public int type;
    public int x;
    public int y;
    public int z;
}
public struct sMap_Seed
{
    public char flag;
    public uint seed;
}
[System.Serializable]
public struct sUserinfo//유저 정보 (idx,id,pass,win,lose)
{
    public int idx;
    public string id;
    public string pass;
    public int Win;
    public int Lose;
}
public struct sMatching//매칭관련
{
    public char flag;
}
public struct sTeam//팀 설정
{
    public char flag;
    public int team;
}
public struct sLogout//로그아웃설정
{
    public char flag;
    public int player;
}
public struct sPosition//위치,각도
{
    public char flag;
    public int player;
    public float px;
    public float py;
    public float pz;
    public float dx;
    public float dy;
    public float dz;
    public float sp;
    public bool fm;
    public bool bm;
    public bool run;
    public bool atk;
}
public struct sRayshot//레이샷
{
    public char flag;
    public int player;
}
public struct sCamera//카메라 움직임
{
    public char flag;
    public int player;
    public float cx;
    public float cy;
    public float cz;
}
public struct sFkey//F키
{
    public char flag;
    public int player;
}
public struct sZkey//Z키
{
    public char flag;
    public int player;
}
public struct sXkey//X키
{
    public char flag;
    public int player;
}
public struct sCkey//C키
{
    public char flag;
    public int player;
}
public struct sVkey//V키
{
    public char flag;
    public int player;
}
public struct sRkey//R키
{
    public char flag;
    public int player;
}
public struct sGamedeath//죽는것 관련
{
    public char flag;
    public int player;
}
public struct sResource//자원
{
    public char flag;
    public int player;
    public int wood;
    public int stone;
}
public struct sStatus//플레이어 상태
{
    public char flag;
    public int player;
    public float hp;
    public int bullet_Min;
    public int bullet_Max;
    public int Mode;
    public int CreateMode2;
    public int MaterialMode;
    public int GunNum;
}
public struct sObject_Tree//나무 상태
{
    public char flag;
    public int status;
    public int number;
}
public struct sObject_Stone//돌 상태
{
    public char flag;
    public int status;
    public int number;
}
public struct sObject_bullet//총알상자 상태
{
    public char flag;
    public int status;
    public int number;
}
public struct sCreateBox_Floor//박스 만들 때
{
    public char flag;
    public int kind;
    public float fx;
    public float fy;
    public float fz;
    public int PlayerNum;
}
public class stateObject//버퍼 관련
{
    public Socket worksocket = null;
    public const int buffersize = 256;
    public byte[] buffer = new byte[buffersize];
    public StringBuilder sb = new StringBuilder();
}
public class Singleton : MonoBehaviour
{
    public static Singleton instance = null;
    Socket C_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    private static ManualResetEvent ConnectDone = new ManualResetEvent(false);
    private static ManualResetEvent SendDone = new ManualResetEvent(false);
    private static ManualResetEvent ReceiveDone = new ManualResetEvent(false);

    private static string response = string.Empty;

    public int Ready = 0;

    public int Rank;//순위
    public int Survive_Number;//살아남은 플레이어 수
    public sUserinfo Userinfo;//유저 정보
    public sLogin Login;//로그인
    public int Team_number;//팀셋팅 시 필요
    public sPosition[] Position = new sPosition[5];//플레이어들 위치
    public sRayshot[] pRayshot = new sRayshot[5];//레이샷 설정
    public sCamera[] pCamera = new sCamera[5];//카메라 설정
    public sFkey[] pFkey = new sFkey[5];
    public sZkey[] pZkey = new sZkey[5];
    public sXkey[] pXkey = new sXkey[5];
    public sVkey[] pVkey = new sVkey[5];
    public sCkey[] pCkey = new sCkey[5];
    public sRkey[] pRkey = new sRkey[5];
    public sGamedeath[] pGamedeath = new sGamedeath[5];
    public sResource[] pResource = new sResource[5];
    public sStatus[] pStatus = new sStatus[5];
    public sObject_Tree pTree = new sObject_Tree();
    public sObject_Stone pStone = new sObject_Stone();
    public sObject_bullet pBullet = new sObject_bullet();
    //public sMap[,,] map = new sMap[25,4,25];
    //public sMap_Seed map_Seed = new sMap_Seed();
    public sCreateBox_Floor CreateBox_Floor=new sCreateBox_Floor();
    public sLogout logout = new sLogout();
    // Start is called before the first frame update
    void Awake()//싱글톤 선언
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        C_Connect();
    }
    void Update()
    {
        instance.Receive_MSG();
    }
    void OnApplicationQuit()
    {
        Close(C_socket);
    }
    void C_Connect()//클라이언트 연결
    {
        var ep = new IPEndPoint(IPAddress.Parse("192.168.0.7"), 10000);//127.0.0.1
        C_socket.BeginConnect(ep, new AsyncCallback(ConnectCallback), C_socket);

    }
    private void Send(Socket client, byte[] buff)//Send
    {
        client.BeginSend(buff, 0, buff.Length, 0, new AsyncCallback(SendCallback), client);
    }
    private void Receive(Socket client)//Receive
    {
        try
        {
            stateObject state = new stateObject();
            state.worksocket = client;
            client.BeginReceive(state.buffer, 0, stateObject.buffersize, 0, new AsyncCallback(ReceiveCallback), state);

        }
        catch (Exception e)
        {
            //Debug.Log(e.ToString());
        }

    }
    private void Close(Socket client)
    {
        client.Shutdown(SocketShutdown.Both);
        client.Close();

    }

    private static void ConnectCallback(IAsyncResult ar)//서버 접속
    {
        try
        {
            Socket client = (Socket)ar.AsyncState;
            client.EndConnect(ar);
            string Logmessage = client.RemoteEndPoint.ToString();
            //Debug.Log("Socket connect to " + Logmessage);
            ConnectDone.Set();
        }
        catch (Exception e)
        {
            //Debug.Log(e.ToString());
        }
    }
    private static void SendCallback(IAsyncResult ar)//보내기
    {
        try
        {
            Socket client = (Socket)ar.AsyncState;
            int bytesent = client.EndSend(ar);
            //Debug.Log("Sent " + bytesent + " bytes to server.");
        }
        catch (Exception e)
        {
            //Debug.Log(e.ToString());
        }
    }
    private static void ReceiveCallback(IAsyncResult ar)//받기
    {
        try
        {
            stateObject state = (stateObject)ar.AsyncState;
            Socket client = state.worksocket;

            int bytesread = client.EndReceive(ar);
            if (bytesread > 0)
            {
                state.sb.Append(Encoding.UTF8.GetString(state.buffer, 0, bytesread));
                client.BeginReceive(state.buffer, 0, stateObject.buffersize, 0, new AsyncCallback(ReceiveCallback), state);

            }
            else
            {
                if (state.sb.Length > 1)
                {
                    response = state.sb.ToString();
                }
                ReceiveDone.Set();
            }
            instance.cdivide(state);

        }
        catch (Exception e)
        {
            //Debug.Log(e.ToString());
        }
    }
    public void cdivide(stateObject state)//플래그에 따라 버퍼를 나눔
    {
        sFlag Flag = new sFlag();
        Flag = (sFlag)BytetoStruct(state.buffer, typeof(sFlag), 256); //플래그 확인
        if (Flag.flag == (char)eMSG.em_USER_LOGIN)
        {
            vLogin(state.buffer);
            //로그인 시 받는 메시지    
        }
        else if (Flag.flag == (char)eMSG.em_USER_MATCHING)
        {
            vMatching();
            //매칭 시 받는 메시지
        }
        else if (Flag.flag == (char)eMSG.em_USER_POSITION)
        {
            Set_Position(state.buffer);
            //위치 받을 시 받는 메시지
        }
        else if (Flag.flag == (char)eMSG.em_USER_TEAM)
        {
            Set_Team(state.buffer);
            //팀 결정 시 받는 메시지
        }
        else if (Flag.flag == (char)eMSG.em_USER_RAYSHOT)
        {
            Set_Rayshot(state.buffer);
            //레이에 관한 메시지
        }
        else if (Flag.flag == (char)eMSG.em_USER_CAMERA)
        {
            Set_Camera(state.buffer);
            //카메라 정보값
        }
        else if (Flag.flag == (char)eMSG.em_USER_FKEY)
        {
            Set_Fkey(state.buffer);
            //Fkey 정보값
        }
        else if (Flag.flag == (char)eMSG.em_USER_ZKEY)
        {
            Set_Zkey(state.buffer);
            //Zkey 정보값
        }
        else if (Flag.flag == (char)eMSG.em_USER_XKEY)
        {
            Set_Xkey(state.buffer);
        }
        else if (Flag.flag == (char)eMSG.em_USER_CKEY)
        {
            Set_Ckey(state.buffer);
            //Fkey 정보값
        }
        else if (Flag.flag == (char)eMSG.em_USER_VKEY)
        {
            Set_Vkey(state.buffer);
            //Fkey 정보값
        }
        else if (Flag.flag == (char)eMSG.em_USER_RKEY)
        {
            Set_Rkey(state.buffer);
            //Rkey 정보값
        }
        else if (Flag.flag == (char)eMSG.em_USER_GAMEDEATH)
        {
            Set_Gamedeath(state.buffer);
        }
        else if (Flag.flag == (char)eMSG.em_USER_RESOURCE)
        {
            Set_Resource(state.buffer);//자원값
        }
        else if (Flag.flag == (char)eMSG.em_USER_STATUS)
        {
            Set_status(state.buffer);//상태값
        }
        else if (Flag.flag == (char)eMSG.em_OBJECT_TREE)
        {
            Set_Tree(state.buffer);//나무 상태값
        }
        else if (Flag.flag == (char)eMSG.em_OBJECT_STONE)
        {
            Set_Stone(state.buffer);//돌 상태값
        }
        else if (Flag.flag == (char)eMSG.em_OBJECT_BULLET)
        {
            Set_Bullet(state.buffer);//총알상자 상태값
        }
        else if (Flag.flag == (char)eMSG.em_MAP_MAPSETTING)
        {
            //set_Mapsetting(state.buffer);
        }
        else if (Flag.flag == (char)eMSG.em_OBJECT_FLOOR)
        {
            Set_Floor(state.buffer);//박스 상태값
        }
        else if (Flag.flag == (char)eMSG.em_USER_LOGOUT)
        {
            //게임도중 로그아웃시 설정
            Set_Logout(state.buffer);
        }
    }

    public static object BytetoStruct(byte[] buffer, Type type, int size)//Byte를 Struct형태로 바꾸는 함수
    {
        IntPtr buff = Marshal.AllocHGlobal(buffer.Length);//배열의 크기만큼 메모리 생성
        Marshal.Copy(buffer, 0, buff, size);
        object obj = Marshal.PtrToStructure(buff, type);
        Marshal.FreeHGlobal(buff);
        return obj;
    }
    public static byte[] StructureToByte(object obj)//Struct를 Byte형태로 바꾸는 함수
    {
        int datasize = Marshal.SizeOf(obj);
        IntPtr buff = Marshal.AllocHGlobal(datasize);
        Marshal.StructureToPtr(obj, buff, false);
        byte[] data = new byte[datasize];
        Marshal.Copy(buff, data, 0, datasize);
        Marshal.FreeHGlobal(buff);
        return data;
    }
    public void Send_LoginMSG()//로그인시 보내는 메시지
    {
        sLogin login = new sLogin();
        login.flag = (char)eMSG.em_USER_LOGIN;
        login.idx = Userinfo.idx;
        byte[] data = StructureToByte(login);
        Send(C_socket, data);

    }
    public void Receive_MSG()//받기
    {
        Receive(C_socket);
        ReceiveDone.Set();
    }
    public void Send_MatchingMSG()//매칭시 보내는 메시지
    {
        sMatching Matching = new sMatching();
        Matching.flag = (char)eMSG.em_USER_MATCHING;
        byte[] data = StructureToByte(Matching);
        Send(C_socket, data);
    }
    public void Send_TeamMSG()//팀 설정시 보내는 메시지
    {
        sTeam Team = new sTeam();
        Team.flag = (char)eMSG.em_USER_TEAM;
        byte[] data = StructureToByte(Team);
        Send(C_socket, data);
    }
    public void Send_Rayshot(int player)//레이샷 설정 시 보내는 메시지
    {
        sRayshot sRay = new sRayshot();
        sRay.flag = (char)eMSG.em_USER_RAYSHOT;
        sRay.player = player;
        byte[] data = StructureToByte(sRay);
        Send(C_socket, data);
    }
    public void Send_PositionMSG(int player, float px, float py, float pz, float dx, float dy, float dz, float sp, bool fm, bool bm, bool run,bool atk)//위치, 움직임관련 메시지
    {
        sPosition spo = new sPosition();
        spo.flag = (char)eMSG.em_USER_POSITION;
        spo.player = player;
        spo.px = px;
        spo.py = py;
        spo.pz = pz;
        spo.dx = dx;
        spo.dy = dy;
        spo.dz = dz;
        spo.sp = sp;
        spo.fm = fm;
        spo.bm = bm;
        spo.run = run;
        spo.atk = atk;
        byte[] data = StructureToByte(spo);
        Send(C_socket, data);
    }
    public void Set_Position(byte[] buffer)//위치 관련 메시지를 받았을 때 설정
    {
        Position[0] = (sPosition)BytetoStruct(buffer, typeof(sPosition), 256);
        for (int pa = 1; pa < 5; pa++)
        {
            if (Position[0].player == pa)
            {
                Position[pa] = Position[0];
                Position[0].player = 0;
            }
        }

    }
    public void Set_Team(byte[] buffer)//팀 관련 메시지를 받았을 때 설정
    {
        sTeam team = new sTeam();
        team = (sTeam)BytetoStruct(buffer, typeof(sTeam), 256);
        Team_number = team.team;
    }
    public void Set_Rayshot(byte[] buffer)//레이 관련 메시지를 받았을 때 설정
    {
        pRayshot[0] = (sRayshot)BytetoStruct(buffer, typeof(sRayshot), 256);
        for (int pa = 1; pa < 5; pa++)
        {
            if (pRayshot[0].player == pa)
            {
                pRayshot[pa] = pRayshot[0];
                pRayshot[0].player = 0;
            }
        }
    }
    public void Rayshot_Reset(int Player_Number)//레이 관련 설정 리셋.
    {
        pRayshot[Player_Number].player = 0;
    }
    public void vMatching()//매칭관련
    {
        Ready++;
    }
    public void Reset_Ready()//매칭 리셋.
    {
        Ready = 0;
    }
    public void Send_CameraMSG(float dx, float dy, float dz, int player)//카메라 관련 메시지
    {
        sCamera sCa = new sCamera();
        sCa.flag = (char)eMSG.em_USER_CAMERA;
        sCa.player = player;
        sCa.cx = dx;
        sCa.cy = dy;
        sCa.cz = dz;
        byte[] data = StructureToByte(sCa);
        Send(C_socket, data);

    }
    public void Set_Camera(byte[] buffer)//카메라 관련 메시지를 받았을 때 설정
    {
        pCamera[0] = (sCamera)BytetoStruct(buffer, typeof(sCamera), 256);
        for (int pa = 1; pa < 5; pa++)
        {
            if (pCamera[0].player == pa)
            {
                pCamera[pa] = pCamera[0];
                pCamera[0].player = 0;
            }
        }
    }

    //F_KEY
    public void Send_FkeyMSG(int player)//F key 메시지
    {
        sFkey sfk = new sFkey();
        sfk.flag = (char)eMSG.em_USER_FKEY;
        sfk.player = player;
        byte[] data = StructureToByte(sfk);
        Send(C_socket, data);
    }
    public void Set_Fkey(byte[] buffer)//F key 설정
    {
        pFkey[0] = (sFkey)BytetoStruct(buffer, typeof(sFkey), 256);
        for (int pa = 1; pa < 5; pa++)
        {
            if (pFkey[0].player == pa)
            {
                pFkey[pa] = pFkey[0];
                pFkey[0].player = 0;
            }
        }
    }
    public void Reset_Fkey(int Player_Number)//F key reset
    {
        pFkey[Player_Number].player = 0;
    }
    //Z_KEY
    public void Send_ZkeyMSG(int player)//Z key 메시지
    {
        sZkey sZk = new sZkey();
        sZk.flag = (char)eMSG.em_USER_ZKEY;
        sZk.player = player;
        byte[] data = StructureToByte(sZk);
        Send(C_socket, data);
    }
    public void Set_Zkey(byte[] buffer)//Z key 설정
    {
        pZkey[0] = (sZkey)BytetoStruct(buffer, typeof(sZkey), 256);
        for (int pa = 1; pa < 5; pa++)
        {
            if (pZkey[0].player == pa)
            {
                pZkey[pa] = pZkey[0];
                pZkey[0].player = 0;
            }
        }
    }
    public void Reset_Zkey(int Player_Number)//Z key 리셋
    {
        pZkey[Player_Number].player = 0;
    }
    //X_KEY
    public void send_XkeyMSG(int player)//X key 관련 메시지
    {
        sXkey sXk = new sXkey();
        sXk.flag = (char)eMSG.em_USER_XKEY;
        sXk.player = player;
        byte[] data = StructureToByte(sXk);
        Send(C_socket, data);
    }
    public void Set_Xkey(byte[] buffer)//X key 설정
    {
        pXkey[0] = (sXkey)BytetoStruct(buffer, typeof(sXkey), 256);
        for (int pa = 1; pa < 5; pa++)
        {
            if (pXkey[0].player == pa)
            {
                pXkey[pa] = pXkey[0];
                pXkey[0].player = 0;
            }
        }

    }
    public void Reset_Xkey(int Player_Number)//X key 리셋
    {
        pXkey[Player_Number].player = 0;
    }
    //V_KEY
    public void Send_VkeyMSG(int player)//V key 메시지
    {
        sVkey sVk = new sVkey();
        sVk.flag = (char)eMSG.em_USER_VKEY;
        sVk.player = player;
        byte[] data = StructureToByte(sVk);
        Send(C_socket, data);
    }
    public void Set_Vkey(byte[] buffer)//V key 설정
    {
        pVkey[0] = (sVkey)BytetoStruct(buffer, typeof(sVkey), 256);
        for (int pa = 1; pa < 5; pa++)
        {
            if (pVkey[0].player == pa)
            {
                pVkey[pa] = pVkey[0];
                pVkey[0].player = 0;
            }
        }
    }
    public void Reset_Vkey(int Player_Number)//V key 리셋
    {
        pVkey[Player_Number].player = 0;
    }
    //C_KEY
    public void Send_CkeyMSG(int player)//C key 메시지
    {
        sCkey sCk = new sCkey();
        sCk.flag = (char)eMSG.em_USER_CKEY;
        sCk.player = player;
        byte[] data = StructureToByte(sCk);
        Send(C_socket, data);
    }
    public void Set_Ckey(byte[] buffer)//C key 설정
    {
        pCkey[0] = (sCkey)BytetoStruct(buffer, typeof(sCkey), 256);
        for (int pa = 1; pa < 5; pa++)
        {
            if (pCkey[0].player == pa)
            {
                pCkey[pa] = pCkey[0];
                pCkey[0].player = 0;
            }
        }
    }
    public void Reset_Ckey(int Player_Number)//C key 리셋
    {
        pCkey[Player_Number].player = 0;
    }
    public void Send_RkeyMSG(int player)//R key 메시지
    {
        sRkey sRk = new sRkey();
        sRk.flag = (char)eMSG.em_USER_RKEY;
        sRk.player = player;
        byte[] data = StructureToByte(sRk);
        Send(C_socket, data);
    }
    public void Set_Rkey(byte[] buffer)//R key 설정
    {
        pRkey[0] = (sRkey)BytetoStruct(buffer, typeof(sRkey), 256);
        for (int pa = 1; pa < 5; pa++)
        {
            if (pRkey[0].player == pa)
            {
                pRkey[pa] = pRkey[0];
                pRkey[0].player = 0;
            }
        }
    }
    public void Reset_Rkey(int Player_Number)//R key 리셋
    {
        pRkey[Player_Number].player = 0;
    }

    public string Login_Webserver(string id, string pass)//로그인-웹서버(파이썬 서버연결)
    {
        string url = "http://192.168.0.7:10001/Login";

        StringBuilder sendInfo = new StringBuilder();
        sendInfo.Append("id=" + id);
        sendInfo.Append("&password=" + pass);

        HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        byte[] byteArr = UTF8Encoding.UTF8.GetBytes(sendInfo.ToString());
        httpWebRequest.ContentType = "application/x-www-form-urlencoded";
        httpWebRequest.Method = "POST";
        httpWebRequest.ContentLength = byteArr.Length;

        Stream stream = httpWebRequest.GetRequestStream();
        stream.Write(byteArr, 0, byteArr.Length);

        HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        Stream result = httpWebResponse.GetResponseStream();
        StreamReader readerResult = new StreamReader(result, Encoding.Default);

        return readerResult.ReadToEnd();
    }
    public string Signup_Webserver(string id, string pass)//회원가입-웹서버(파이썬 서버 연결)
    {
        string url = "http://192.168.0.7:10001/Signup";

        StringBuilder sendInfo = new StringBuilder();
        sendInfo.Append("id=" + id);
        sendInfo.Append("&password=" + pass);

        HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        byte[] byteArr = UTF8Encoding.UTF8.GetBytes(sendInfo.ToString());
        httpWebRequest.ContentType = "application/x-www-form-urlencoded";
        httpWebRequest.Method = "POST";
        httpWebRequest.ContentLength = byteArr.Length;

        Stream stream = httpWebRequest.GetRequestStream();
        stream.Write(byteArr, 0, byteArr.Length);

        HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        Stream result = httpWebResponse.GetResponseStream();
        StreamReader readerResult = new StreamReader(result, Encoding.Default);

        return readerResult.ReadToEnd();
    }
    public string GameOver_Webserver(string id, int lose)//게임오버-웹서버(파이썬 서버 연결) lose 갱신
    {
        string url = "http://192.168.0.7:10001/GameOver";

        StringBuilder sendInfo = new StringBuilder();
        sendInfo.Append("id=" + id);
        sendInfo.Append("&lose=" + lose);

        HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        byte[] byteArr = UTF8Encoding.UTF8.GetBytes(sendInfo.ToString());
        httpWebRequest.ContentType = "application/x-www-form-urlencoded";
        httpWebRequest.Method = "POST";
        httpWebRequest.ContentLength = byteArr.Length;

        Stream stream = httpWebRequest.GetRequestStream();
        stream.Write(byteArr, 0, byteArr.Length);

        HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        Stream result = httpWebResponse.GetResponseStream();
        StreamReader readerResult = new StreamReader(result, Encoding.Default);

        return readerResult.ReadToEnd();
    }
    public string GameWin_Webserver(string id, int win)//게임승리-웹서버(파이썬 서버연결) win 갱신
    {
        string url = "http://192.168.0.7:10001/GameWin";

        StringBuilder sendInfo = new StringBuilder();
        sendInfo.Append("id=" + id);
        sendInfo.Append("&win=" + win);

        HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        byte[] byteArr = UTF8Encoding.UTF8.GetBytes(sendInfo.ToString());
        httpWebRequest.ContentType = "application/x-www-form-urlencoded";
        httpWebRequest.Method = "POST";
        httpWebRequest.ContentLength = byteArr.Length;

        Stream stream = httpWebRequest.GetRequestStream();
        stream.Write(byteArr, 0, byteArr.Length);

        HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        Stream result = httpWebResponse.GetResponseStream();
        StreamReader readerResult = new StreamReader(result, Encoding.Default);

        return readerResult.ReadToEnd();
    }
    public void Login_success(string msg)//로그인 성공시
    {
        Userinfo = JsonUtility.FromJson<sUserinfo>(msg);

    }
    public void vLogin(byte[] buffer)//로그인
    {
        Login = (sLogin)BytetoStruct(buffer, typeof(sLogin), 256);
    }
    public void Send_Gamedeath(int player)//게임죽음 메시지
    {
        sGamedeath sGd = new sGamedeath();
        sGd.flag = (char)eMSG.em_USER_GAMEDEATH;
        sGd.player = player;
        byte[] data = StructureToByte(sGd);
        Send(C_socket, data);
    }
    public void Set_Gamedeath(byte[] buffer)//게임 죽음 설정
    {
        pGamedeath[0] = (sGamedeath)BytetoStruct(buffer, typeof(sGamedeath), 256);
        for (int pa = 1; pa < 5; pa++)
        {
            if (pGamedeath[0].player == pa)
            {
                pGamedeath[pa] = pGamedeath[0];
                pGamedeath[0].player = 0;
            }
        }
    }
    public void Reset_Gamedeath(int Player_Number)//죽음메시지 리셋
    {
        pGamedeath[Player_Number].player = 0;
    }
    public void Start_rank()//게임시작시 랭크설정
    {
        Rank = 4;
        Survive_Number = 4;
    }
    public void Set_rank()//랭크 설정
    {
        Rank = Survive_Number;
        Survive_Number--;
    }
    public void Set_win()//이겼을시 랭크 설정
    {
        Rank = Survive_Number;
    }
    public void Send_Resource(int Player_Number, int Wood, int Stone)//자원 메시지
    {
        sResource sRs = new sResource();
        sRs.flag = (char)eMSG.em_USER_RESOURCE;
        sRs.player = Player_Number;
        sRs.wood = Wood;
        sRs.stone = Stone;
        byte[] data = StructureToByte(sRs);
        Send(C_socket, data);
    }
    public void Set_Resource(byte[] buffer)//자원 설정
    {
        pResource[0] = (sResource)BytetoStruct(buffer, typeof(sResource), 256);
        for (int pa = 1; pa < 5; pa++)
        {
            if (pResource[0].player == pa)
            {
                pResource[pa] = pResource[0];
                pResource[0].player = 0;
            }
        }
    }
    public void Reset_Resource(int Player_number)//자원 메시지 리셋
    {
        pResource[Player_number].player = 0;
    }
    public void Send_Status(int Player_Number, float hp, int bullet_Min, int bullet_Max, int Mode, int CreateMode2, int MaterialMode,int GunNum)//상태메시지
    {
        sStatus sSt = new sStatus();
        sSt.flag = (char)eMSG.em_USER_STATUS;
        sSt.player = Player_Number;
        sSt.hp = hp;
        sSt.bullet_Min = bullet_Min;
        sSt.bullet_Max = bullet_Max;
        sSt.Mode = Mode;
        sSt.CreateMode2 = CreateMode2;
        sSt.MaterialMode = MaterialMode;
        sSt.GunNum = GunNum;
        byte[] data = StructureToByte(sSt);
        Send(C_socket, data);
    }
    public void Set_status(byte[] buffer)//상태메시지 설정
    {
        pStatus[0] = (sStatus)BytetoStruct(buffer, typeof(sStatus), 256);
        for (int pa = 1; pa < 5; pa++)
        {
            if (pStatus[0].player == pa)
            {
                pStatus[pa] = pStatus[0];
                pStatus[0].player = 0;
            }
        }
    }
    public void Reset_status(int Player_number)//상태메시지 리셋
    {
        pStatus[Player_number].player = 0;
    }
    public void Reset_Camera(int Player_number)//카메라 메시지 리셋
    {
        pCamera[Player_number].player = 0;
    }
    public void Send_Tree(int number)//나무 메시지 메시지
    {
        sObject_Tree sOT = new sObject_Tree();
        sOT.flag = (char)eMSG.em_OBJECT_TREE;
        sOT.status = 1;
        sOT.number = number;
        byte[] data = StructureToByte(sOT);
        Send(C_socket, data);
    }
    public void Set_Tree(byte[] buffer)//나무 설정
    {
        pTree = (sObject_Tree)BytetoStruct(buffer, typeof(sObject_Tree), 256);
    }
    public void Reset_Tree()//나무 메시지 리셋
    {
        pTree.status = 0;
    }
    public void Send_Stone(int number)//돌 메시지
    {
        sObject_Stone sOS = new sObject_Stone();
        sOS.flag = (char)eMSG.em_OBJECT_STONE;
        sOS.status = 1;
        sOS.number = number;
        byte[] data = StructureToByte(sOS);
        Send(C_socket, data);
    }
    public void Set_Stone(byte[] buffer)//돌 설정
    {
        pStone = (sObject_Stone)BytetoStruct(buffer, typeof(sObject_Stone), 256);
    }
    public void Reset_Stone()//돌 메시지 리셋
    {
        pStone.status = 0;
    }
    public void Send_Bullet(int number)//총알박스 메시지
    {
        sObject_bullet sOB = new sObject_bullet();
        sOB.flag = (char)eMSG.em_OBJECT_BULLET;
        sOB.number = number;
        sOB.status = 1;
        byte[] data = StructureToByte(sOB);
        Send(C_socket, data);
    }
    public void Set_Bullet(byte[] buffer)//총알박스 설정
    {
        pBullet = (sObject_bullet)BytetoStruct(buffer, typeof(sObject_bullet), 256);
    }
    public void Reset_Bullet()//총알박스 메시지 리셋
    {
        pBullet.status = 0;
    }
    /*public void set_Mapsetting(byte[] buffer)
    {
        //map_Seed = (sMap_Seed)BytetoStruct(buffer, typeof(sMap_Seed), 256);
    }
    public void Send_Map()
    {
        sMap_Seed smp = new sMap_Seed();
        smp.flag = (char)eMSG.em_MAP_MAPSETTING;
        byte[] data = StructureToByte(smp);
        Send(C_socket, data);
    }
    public void buildmap()
    {
        int seed = 1;
        System.Random rnd = new System.Random(seed);
        //지형 세팅
        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 25; x++)
            {
                for (int z = 0; z < 25; z++)
                {
                    map[x, y, z].x = x;
                    map[x, y, z].y = y;
                    map[x, y, z].z = z;
                    map[x, y, z].type = 0;
                    if (y == 0)
                    {
                        map[x, y, z].type = 1;
                    }
                    else if (y >= 1)
                    {
                        int ran = rnd.Next(0,100);
                        if (y == 1 && map[x, y - 1, z].type == 1)
                        {
                            if (ran < 40)
                            {
                                map[x, y, z].type = 2;
                            }
                        }
                        else if (y == 2 && map[x, y - 1, z].type == 2)
                        {
                            if (ran < 20)
                            {
                                map[x, y, z].type = 3;
                            }
                        }


                    }
                }
            }
        }
        //오브젝트 셋팅
        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 25; x++)
            {
                for (int z = 0; z < 25; z++)
                {
                }
            }
        }
    }*/
    public void Send_Floor(int type,float fx,float fy,float fz,int Player_Num)//박스 메시지
    {
        sCreateBox_Floor SCF = new sCreateBox_Floor();
        SCF.flag = (char)eMSG.em_OBJECT_FLOOR;
        SCF.fx = fx;
        SCF.fy = fy;
        SCF.fz = fz;
        SCF.kind = type;
        SCF.PlayerNum = Player_Num;
        byte[] data = StructureToByte(SCF);
        Send(C_socket, data);
    }
    public void Set_Floor(byte[] buffer)//박스 메시지 설정
    {
        CreateBox_Floor = (sCreateBox_Floor)BytetoStruct(buffer, typeof(sCreateBox_Floor), 256);
    }
    public void Reset_Floor()//박스 메시지 리셋
    {
        CreateBox_Floor.kind = 0;
    }
    public void Set_Logout(byte[] buffer)
    {
        logout = (sLogout)BytetoStruct(buffer, typeof(sLogout), 256);
    }
    public void Reset_logout()
    {
        logout.player = 0;
    }
}
