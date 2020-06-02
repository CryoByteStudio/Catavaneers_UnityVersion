using UnityEngine;


namespace Learning.Utils
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

        #endregion
    }
}
