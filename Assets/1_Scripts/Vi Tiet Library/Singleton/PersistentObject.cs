using UnityEngine;

namespace Catavaneer.Singleton
{
    public class PersistentObject : SingletonEntity<PersistentObject>
    {
        [SerializeField] private bool isSingleton;
        public bool IsSingleTon => isSingleton;

        #region UNITY ENGINE FUNCTION

        override protected void Awake()
        {
            if (isSingleton)
                base.Awake();

            DontDestroyOnLoad(gameObject);
        }

        private void OnValidate()
        {
            if (isSingleton)
            {
                PersistentObject[] persistentObjects = FindObjectsOfType<PersistentObject>();
                int thisIndex = int.MaxValue;
                int smallestIndex = int.MaxValue;
                int singletonCount = 0;

                for (int i = 0; i < persistentObjects.Length; i++)
                {
                    if (persistentObjects[i] == this)
                    {
                        thisIndex = i;
                    }

                    if (persistentObjects[i].isSingleton)
                    {
                        smallestIndex = smallestIndex < i ? smallestIndex : i;
                        singletonCount++;
                    }
                }
                
                if (singletonCount > 1)
                {
                    Debug.Log(name + ": More than one persistent objects are set to singleton. Only " + persistentObjects[thisIndex > smallestIndex ? smallestIndex : thisIndex] + " instance will be kept at runtime.");
                }
            }
        }

        #endregion
    }
}