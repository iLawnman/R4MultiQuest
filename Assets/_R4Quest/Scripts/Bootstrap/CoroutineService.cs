using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CoroutineService : MonoBehaviour
{
    public static CoroutineService Instance;

    private void Start()
    {
        Instance = this;
    }

    public Coroutine RunCoroutine(IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }

    // Остановка корутины
    public void StopCoroutineExternal(Coroutine coroutine)
    {
        StopCoroutine(coroutine);
    }
}