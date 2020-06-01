using UnityEngine;

namespace Learning.Singleton
{
    public class PersistentObject : SingletonEntity<PersistentObject>
    {
        [SerializeField] private bool isSingleton;

        #region UNITY ENGINE FUNCTION

        override protected void Awake()
        {
            if (isSingleton)
                base.Awake();

            DontDestroyOnLoad(gameObject);
        }

        #endregion
    }
}