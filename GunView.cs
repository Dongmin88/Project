using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunView : MonoBehaviour
{
    public GameObject M4;
    public GameObject Ak;
    public GameObject Revolver;

    public void ViewGun(int GunNum) // 총기번호를 넘겨받아 해당 총기 출력
    {
        if (GunNum==1)
        {
            Ak.SetActive(true);
        }
        else
        {
            Ak.SetActive(false);
        }
        if (GunNum==2)
        {
            M4.SetActive(true);
        }
        else
        {
            M4.SetActive(false);
        }
        if (GunNum==3)
        {
            Revolver.SetActive(true);
        }
        else
        {
            Revolver.SetActive(false);
        }
    }
}
