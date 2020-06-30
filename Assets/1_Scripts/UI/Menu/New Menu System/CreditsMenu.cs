using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ViTiet.Utils;
using Catavaneer.Extensions;
using DG.Tweening;

namespace Catavaneer.MenuSystem
{
    public class CreditsMenu : Menu<CreditsMenu>
    {
        [SerializeField] private GameObject backButton;
        [SerializeField] private float backDelay;
        [SerializeField] private List<MaskableGraphic> graphicsToFade;
        private float backTimer;

        #region UNITY ENGINE FUNCTIONS
        private void OnEnable()
        {
            BackButtonFadeOn();
        }

        //private void OnEnable()
        //{
        //    backTimer = Time.time + backDelay;
        //}

        //private void Update()
        //{
        //    if (Input.anyKeyDown && Time.time >= backTimer)
        //    {
        //        OnBackPressed();
        //    }
        //}
        #endregion

        #region PRIVATE METHODS
        private void BackButtonFadeOn()
        {
            if (backButton)
                graphicsToFade = backButton.transform.GetAllComponentsOfTypeInHierachy<MaskableGraphic>();

            if (graphicsToFade != null && graphicsToFade.Count > 0)
            {
                float delay = backDelay / 3f;
                foreach (var graphic in graphicsToFade)
                {
                    graphic.DOFade(0.75f, backDelay - delay).From(0).SetDelay(delay)
                        .OnComplete(() => EventSystem.current.SetSelectedGameObject(backButton));
                }
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