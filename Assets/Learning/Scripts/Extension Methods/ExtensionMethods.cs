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
            if (list == null || list.Count <= 0)
            {
                list = new List<T>();
            }

            if (addRoot)
            {
                T rootComponent = root.GetComponent<T>();

                if (rootComponent)
                {
                    list.Add(rootComponent);
                }
            }

            foreach (Transform child in root)
            {
                GetAllComponentsOfTypeInHierachy(child, list, false);

                T childComponent = child.GetComponent<T>();

                if (childComponent)
                {
                    list.Add(childComponent);
                }
            }

            return list;
        }

        #endregion
    }
}