using UnityEngine;
using UnityEngine.SceneManagement;
using Learning.LevelManagement;

namespace Learning.Singleton
{
    public abstract class SingletonEntity<T> : MonoBehaviour where T : SingletonEntity<T>
    {
        protected static T instance;
        public static T Instance { get { return instance; } }

        #region UNITY ENGINE FUNCTION

        virtual protected void Awake()
        {
            if (instance)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = (T)this;
            }
        }

        virtual protected void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }

        virtual protected void OnEnable()
        {
            LevelLoader.SceneLoaded += SceneLoadedHandler;
        }

        virtual protected void SceneLoadedHandler(Scene scene, LoadSceneMode mode)
        {
            LevelLoader.ResetLoadingParams();
        }

        virtual protected void OnDisable()
        {
            LevelLoader.SceneLoaded -= SceneLoadedHandler;
        }

        #endregion
    }
}