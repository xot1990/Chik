using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCtrl : MonoBehaviour
{
    public AudioSource music;
    public AudioSource sound;

    public GameObject offS;
    public GameObject onS;
    public GameObject offM;
    public GameObject onM;

    public void OnM()
    {
        music.volume = 1;
    }

    public void OffM()
    {
        music.volume = 0;
    }

    public void OnS()
    {
        sound.volume = 1;
    }

    public void OffS()
    {
        sound.volume = 0;
    }
}
