using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TestEnumConvert))]
public class TestEnumConvertEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TestEnumConvert enumConvert = target as TestEnumConvert;

        if (GUILayout.Button("Convert Int"))
        {
            enumConvert.ConvertIntToEnum();
        }

        if (GUILayout.Button("Convert String"))
        {
            enumConvert.ConvertStringToEnum();
        }
    }
}
