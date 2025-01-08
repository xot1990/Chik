using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameManager gameManager;
    public Screenmanager screenmanager;

    public void StartLvl(int lvl)
    {
        gameManager.currentLevel = lvl;
        gameManager.StartGame();
        screenmanager.StepToPlay();
    }
}
