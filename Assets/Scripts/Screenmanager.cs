using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenmanager : MonoBehaviour
{
    public GameObject Menu;
    public GameObject Levels;
    public GameObject Rules;
    public GameObject Info;

    public void StepToMenu()
    {
        Levels.SetActive(false);
        Rules.SetActive(false);
        Info.SetActive(false);
        Menu.SetActive(true);
    }
}
