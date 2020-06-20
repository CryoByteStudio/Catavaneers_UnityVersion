using UnityEngine;
using UnityEngine.EventSystems;
using ViTiet.Utils;

namespace Catavaneer.MenuSystem
{
    public class CreditsMenu : Menu<CreditsMenu>
    {
        [SerializeField]
        private GameObject firstSelected;

        #region UNITY ENGINE FUNCTIONS
        protected override void Awake()
        {
            base.Awake();
            SetSelectedGameObject(firstSelected);
        }

        private void Update()
        {
            if (Input.anyKeyDown)
            {
                OnBackPressed();
            }
        }
        #endregion

        #region PUBLIC METHODS
        public override void OnBackPressed()
        {
            base.OnBackPressed();
        }
        #endregion
    }
}