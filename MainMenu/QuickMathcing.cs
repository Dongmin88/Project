using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickMathcing : MonoBehaviour
{
    public Button HowtoPlay;
    public Button Quit;
    public void QuickMatching_click()
    {
        Singleton.instance.Send_MatchingMSG();
        transform.GetComponent<Button>().interactable = false;
        HowtoPlay.interactable = false;
        Quit.interactable = false;
    }
}
