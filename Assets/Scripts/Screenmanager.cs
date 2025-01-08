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

    public void StepToLevels()
    {
        Levels.SetActive(true);
        Rules.SetActive(false);
        Info.SetActive(false);
        Menu.SetActive(false);
    }

    public void StepToRules()
    {
        Levels.SetActive(false);
        Rules.SetActive(true);
        Info.SetActive(false);
        Menu.SetActive(false);
    }

    public void StepToInfo()
    {
        Levels.SetActive(false);
        Rules.SetActive(false);
        Info.SetActive(true);
        Menu.SetActive(false);
    }

    public void StepToPlay()
    {
        Levels.SetActive(false);
        Rules.SetActive(false);
        Info.SetActive(false);
        Menu.SetActive(false);
    }
}
