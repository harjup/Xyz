using System;
using UnityEngine;
using System.Collections;

public static class UsefulCoroutines
{
    public static IEnumerator ExecuteAfterDelay(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }
}
