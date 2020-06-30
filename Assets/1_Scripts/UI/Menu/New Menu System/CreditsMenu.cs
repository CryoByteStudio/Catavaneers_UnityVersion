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
        [Header("Back Button Fade On Settings")]
        [SerializeField] private GameObject backButton;
        [SerializeField] private float backDelay;
        [SerializeField] private List<MaskableGraphic> buttonGraphics;

        [Header("Credit Roll Settings")]
        [SerializeField] private float moveDistance;
        [SerializeField] private float duration;
        [SerializeField] private float delay;
        [SerializeField] private Transform creditRollTransform;
        [SerializeField] private List<MaskableGraphic> textGraphics;
        [SerializeField] private MaskableGraphic endingText;

        private Vector3 startPosition;
        
        private float backTimer;

        #region UNITY ENGINE FUNCTIONS
        private void OnEnable()
        {
            startPosition = creditRollTransform.position;
        }
        #endregion

        #region PUBLIC METHODS
        public void Play()
        {
            BackButtonFadeOn();

            foreach (var graphic in textGraphics)
            {
                graphic.DOFade(1, delay).From(0);
            }

            creditRollTransform.DOMoveY(moveDistance, duration).SetEase(Ease.Linear).SetDelay(delay);
            endingText.DOFade(1, 1).SetDelay(duration);
        }

        private void BackButtonFadeOn()
        {
            if (backButton)
                buttonGraphics = backButton.transform.GetAllComponentsOfTypeInHierachy<MaskableGraphic>();

            if (buttonGraphics != null && buttonGraphics.Count > 0)
            {
                float delay = backDelay / 3f;
                foreach (var graphic in buttonGraphics)
                {
                    graphic.DOFade(0.75f, backDelay - delay).From(0).SetDelay(delay)
                        .OnComplete(() => EventSystem.current.SetSelectedGameObject(backButton));
                }
            }
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            Reset();
        }

        private void Reset()
        {
            if (buttonGraphics != null && buttonGraphics.Count > 0)
            {
                foreach (var graphic in buttonGraphics)
                {
                    graphic.DOFade(0, 0);
                }
            }

            creditRollTransform.position = startPosition;
        }
        #endregion
    }
}