using UnityEngine;
using UnityEngine.EventSystems;
using ViTiet.Utils;

namespace Catavaneer.MenuSystem
{
    public class CreditsMenu : Menu<CreditsMenu>
    {
        [SerializeField]
        private GameObject firstSelected;
        //private EventSystem eventSystem;

        #region UNITY METHODS
        private void Update()
        {
            if (Input.anyKeyDown)
            {
                OnBackPressed();
            }
        }

        //private void OnEnable()
        //{
        //    if (!eventSystem)
        //        eventSystem = FindObjectOfType<EventSystem>();

        //    if (!eventSystem)
        //    {
        //        EditorHelper.ArgumentNullException("eventSystem");
        //        return;
        //    }

        //    eventSystem.firstSelectedGameObject = firstSelected;
        //}

        //private void OnDisable()
        //{
        //    eventSystem.firstSelectedGameObject = null;
        //    eventSystem = null;
        //}
        #endregion

        #region PUBLIC METHODS
        public override void OnBackPressed()
        {
            base.OnBackPressed();
        }
        #endregion
    }
}