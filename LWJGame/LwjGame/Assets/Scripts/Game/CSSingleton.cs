using UnityEngine;
using System.Collections;
using System;

public class CSSingleton<T> : MonoBehaviour {

    private static T Instance;
    public static T mInstance
    {
        get {
            if (Instance == null)
            {
                Instance = Activator.CreateInstance<T>();
            }

            return Instance;
        }
    }
}
