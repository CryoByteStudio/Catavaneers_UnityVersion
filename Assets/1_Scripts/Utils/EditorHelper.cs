using System;
using UnityEngine;


namespace Catavaneer.Utils
{
    public static class EditorHelper
    {
        #region PUBLIC STATIC METHODS

        public static void Log(object caller, string message)
        {
            if (caller != null)
            {
                Debug.Log("[" + caller + "]: " + message);
            }
            else
            {
                Debug.Log(message);
            }
        }

        public static void LogWarning(object caller, string message)
        {
            if (caller != null)
            {
                Debug.LogWarning("[" + caller + "]: " + message);
            }
            else
            {
                Debug.LogWarning(message);
            }
        }

        public static void LogError(object caller, string message)
        {
            if (caller != null)
            {
                Debug.LogError("[" + caller + "]: " + message);
            }
            else
            {
                Debug.LogError(message);
            }
        }

        public static bool IsValueNull(object obj, string objName)
        {
            if (obj == null)
            {
                LogWarning(null, "[" + objName + "] instance is null!");
                return true;
            }

            return false;
        }

        public static void NotImplementedException(string message = null)
        {
            if (!string.IsNullOrEmpty(message))
                throw new NotImplementedException(message);
            else
                throw new NotImplementedException();
        }

        public static void ArgumentNullException(string paramName = null)
        {
            if (!string.IsNullOrEmpty(paramName))
                throw new ArgumentNullException(paramName);
            else
                throw new ArgumentNullException();
        }

        public static void NotSupportedException(string message = null)
        {
            if (!string.IsNullOrEmpty(message))
                throw new NotSupportedException(message);
            else
                throw new NotSupportedException();
        }

        #endregion
    }
}
