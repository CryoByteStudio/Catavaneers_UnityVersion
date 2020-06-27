using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WorldTextStartForce))]
public class WorldTextStartForceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WorldTextStartForce worldTextStartForceObject = target as WorldTextStartForce;

        if (GUILayout.Button("Push"))
        {
            worldTextStartForceObject.Push();
        }

        if (GUILayout.Button("Randomize Force Direction"))
        {
            worldTextStartForceObject.RandomizeForceDirection();
        }
    }
}