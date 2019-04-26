using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HowTPclick : MonoBehaviour
{
    public void Howtoplay_click()
    {
        SceneManager.LoadScene("HowToPlay");
    }
}
