using Catavaneer.MenuSystem;
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

    private LoseTransitionFader test;
    private void Start()
    {
        //var testArray = Resources.FindObjectsOfTypeAll<LoseTransitionFader>();
        //if (testArray.Length > 0)
        //{
        //    test = testArray[0];
        //    if (test)
        //    {
        //        test.gameObject.SetActive(true);
        //        test.Play();
        //    }
        //}

        MenuManager.Instance.OpenLoseMenu();
    }
}
