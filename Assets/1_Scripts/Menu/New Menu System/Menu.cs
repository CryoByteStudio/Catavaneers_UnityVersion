using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Catavaneer.MenuSystem
{
    [RequireComponent(typeof(Canvas))]
    public abstract class Menu<T> : Menu where T : Menu<T>
    {
        private static T instance;
        public static T Instance { get { return instance; } }

        #region UNITY ENGINE FUNCTIONS

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
            instance = null;
        }

        #endregion

        #region PUBLIC STATIC METHODS

        public static void Open()
        {
            if (MenuManager.Instance && instance)
            {
                MenuManager.OpenMenu(instance);
            }
        }

        #endregion
    }

    [RequireComponent(typeof(Canvas))]
    public abstract class Menu : MonoBehaviour
    {
        public GameObject selectedGameObject { get; private set; }

        #region PUBLIC METHODS
        public virtual void OnBackPressed()
        {
            if (MenuManager.Instance)
            {
                MenuManager.CloseMenu();
            }
        }

        public virtual void SetSelectedGameObject(GameObject selectedGameObject)
        {
            this.selectedGameObject = selectedGameObject;
        }
        #endregion
    }
}