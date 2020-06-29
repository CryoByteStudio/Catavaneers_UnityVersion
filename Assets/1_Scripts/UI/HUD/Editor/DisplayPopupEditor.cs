using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DisplayPopup))]
public class DisplayPopupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DisplayPopup displayPopup = target as DisplayPopup;

        if (GUILayout.Button("Animate"))
        {
            displayPopup.PlayTest();
        }
        if (GUILayout.Button("Reset"))
        {
            displayPopup.Reset();
        }
    }
}