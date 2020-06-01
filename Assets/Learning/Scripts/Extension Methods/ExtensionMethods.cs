using System.Collections.Generic;
using UnityEngine;

namespace Learning.Extensions
{
    public static class ExtensionMethods
    {
        #region PUBLIC STATIC METHODS

        public static List<Transform> GetAllTransformInHierachy(this Transform root, List<Transform> transformList = null)
        {
            if (transformList == null)
                transformList = new List<Transform>();

            foreach (Transform branch in root)
            {
                GetAllTransformInHierachy(branch, transformList);
                transformList.Add(branch);
            }

            return transformList;
        }

        public static List<T> GetAllComponentsOfTypeInHierachy<T> (this Transform root, List<T> list = null, bool addRoot = true) where T : MonoBehaviour
        {
            T item;

            if (list == null)
            {
                list = new List<T>();
            }

            if (addRoot)
            {
                item = root.GetComponent<T>();

                if (item)
                {
                    list.Add(item);
                }
            }

            foreach (Transform child in root)
            {
                GetAllComponentsOfTypeInHierachy(child, list, false);

                item = child.GetComponent<T>();

                if (item)
                {
                    list.Add(item);
                }
            }

            return list;
        }

        #endregion
    }
}