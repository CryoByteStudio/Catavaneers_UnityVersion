using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static List<T> GetAllComponentsOfType<T>(this Transform root, List<T> list = null, bool addRoot = true) where T : MonoBehaviour
    {
        if (list == null || list.Count <= 0)
            list = new List<T>();


        if (addRoot)
        {
            T rootTComponent = root.GetComponent<T>();

            if (rootTComponent)
                list.Add(rootTComponent);
        }

        foreach (Transform child in root)
        {
            T item = child.GetComponent<T>();

            if (item)
                list.Add(item);

            GetAllComponentsOfType(child, list, false);
        }

        return list;
    }
}
