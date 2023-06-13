using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenController : MonoBehaviour
{
    [SerializeField] int targetFPS = 60; // •ÏX‚µ‚½‚¢–Ú•W‚ÌFPS’l

    void Start()
    {
        Application.targetFrameRate = targetFPS;
    }
}
