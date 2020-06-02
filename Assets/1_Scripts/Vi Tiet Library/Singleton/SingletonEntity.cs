using UnityEngine;
using UnityEngine.SceneManagement;
using Catavaneer.LevelManagement;

namespace Catavaneer.Singleton
{
    public abstract class SingletonEntity<T> : MonoBehaviour where T : SingletonEntity<T>
    {
        protected static T instance;
        public static T Instance { get { return instance; } }

        #region UNITY ENGINE FUNCTION

        protected virtual void Awake()
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

        protected virtual void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }

        protected virtual void OnEnable()
        {
            LevelLoader.SceneLoaded += SceneLoadedHandler;
        }

        protected virtual void SceneLoadedHandler(Scene scene, LoadSceneMode mode)
        {
            LevelLoader.ResetLoadingParams();
        }

        protected virtual void OnDisable()
        {
            LevelLoader.SceneLoaded -= SceneLoadedHandler;
        }

        #endregion
    }
}