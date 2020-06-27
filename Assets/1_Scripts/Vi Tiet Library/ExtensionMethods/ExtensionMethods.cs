using System;
using System.Collections.Generic;
using UnityEngine;

namespace Catavaneer.Extensions
{
    public static class ExtensionMethods
    {
        #region PUBLIC STATIC METHODS
        public static List<Transform> GetAllTransformInHierachy(this Transform root, List<Transform> transformList = null, bool addRoot = true)
        {
            if (transformList == null)
                transformList = new List<Transform>();


            if (addRoot)
                transformList.Add(root);

            foreach (Transform branch in root)
            {
                transformList.Add(branch);
                GetAllTransformInHierachy(branch, transformList, false);
            }

            return transformList;
        }

        public static List<T> GetAllComponentsOfTypeInHierachy<T>(this Transform root, List<T> genericList = null, bool addRoot = true) where T : UnityEngine.Object
        {
            if (genericList == null || genericList.Count <= 0)
                genericList = new List<T>();

            if (addRoot)
            {
                T rootComponent = root.GetComponent<T>();

                if (rootComponent)
                    genericList.Add(rootComponent);
            }

            foreach (Transform child in root)
            {
                T childComponent = child.GetComponent<T>();

                if (childComponent)
                    genericList.Add(childComponent);

                GetAllComponentsOfTypeInHierachy(child, genericList, false);
            }

            return genericList;
        }

        public static T ToEnum<T>(this string stringValue) where T : struct, IConvertible
        {
            Type enumType = typeof(T);
            if (!enumType.IsEnum)
                throw new Exception("T should be an Enumeration type");
            T enumValue;
            return Enum.TryParse(stringValue, out enumValue) ? enumValue : default;
        }

        public static T ToEnum<T>(this int intValue) where T : struct, IConvertible
        {
            Type enumType = typeof(T);
            if (!enumType.IsEnum)
                throw new Exception("T should be an Enumeration type");
            T enumValue = (T)Enum.ToObject(enumType, intValue);
            return Enum.IsDefined(enumType,enumValue) ? enumValue : default;
        }
        #endregion
    }
}