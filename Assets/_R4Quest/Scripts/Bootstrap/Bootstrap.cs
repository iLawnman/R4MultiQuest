using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using Random = UnityEngine.Random;

public class Bootstrap : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(this);
    }
}