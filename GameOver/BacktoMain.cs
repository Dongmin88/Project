﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BacktoMain : MonoBehaviour
{
    public void BacktoMain_click()
    {
        SceneManager.LoadScene("MainMenu");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
