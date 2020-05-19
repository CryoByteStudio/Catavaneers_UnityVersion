using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NewTest
{
    public static void MyExtensionFunction(this Transform transform)
    {
        Debug.Log("I Made An Extension Function");
    }
}
