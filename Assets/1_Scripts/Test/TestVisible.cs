using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVisible : MonoBehaviour
{
    private void OnBecameInvisible()
    {
        Debug.Log(name + " is invisible");
    }

    private void OnBecameVisible()
    {
        Debug.Log(name + " is visible");
    }
}
