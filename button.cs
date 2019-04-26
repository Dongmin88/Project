using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class button : MonoBehaviour {
    public GameObject brick;
	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}
   public void gamestartbtnClick()
    {
        SceneManager.LoadScene("SampleScene");//게임 화면 로딩
    }
    public void optionbtnClick()
    {
        Instantiate(brick, new Vector3 (0, 0, 0), Quaternion.identity);
    }
    public void quitbtnClick()
    {
        Application.Quit();//app 종료
    }
    public void loginbtnClick()
    {
        //로그인
    }
}
