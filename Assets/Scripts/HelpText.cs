using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpText : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject,1);
    }

    void Update()
    {
        transform.Translate(Vector2.up*Time.deltaTime);
    }
}
