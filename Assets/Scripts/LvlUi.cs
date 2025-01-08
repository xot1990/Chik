using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LvlUi : MonoBehaviour
{
    public GameObject close;
    public int Num;
    public TMP_Text openNum;
    public TMP_Text closeNum;

    private void OnEnable()
    {
        openNum.text = Num.ToString();
        closeNum.text = Num.ToString();
        
        if (PlayerPrefs.GetInt("LVL" + Num + "Open", 0) == 0)
        {
            close.SetActive(true);
        }
        else
        {
            close.SetActive(false);
        }

        if (Num == 1)
        {
            close.SetActive(false);
        }
            
    }
}
