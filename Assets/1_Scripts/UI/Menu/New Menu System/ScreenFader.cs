using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Catavaneer.Extensions;
using DG.Tweening;

namespace Catavaneer.MenuSystem
{
    public class ScreenFader : MonoBehaviour
    {
        protected const float SOLID_APLHA = 1f;
        protected const float CLEAR_ALPHA = 0f;

        [SerializeField] private float fadeOnDuration = 0f;
        [SerializeField] private float fadeOffDuration = 0f;
        protected List<MaskableGraphic> graphicsToFade;

        public float FadeOnDuration { get { return fadeOnDuration; } }
        public float FadeOffDuration { get { return fadeOffDuration; } }
        public float FadeDuration { get { return fadeOnDuration + fadeOffDuration; } }

        #region UNITY ENGINE FUNCTION
        protected virtual void Awake()
        {
            graphicsToFade = transform.GetAllComponentsOfTypeInHierachy<MaskableGraphic>();
        }
        #endregion

        #region PRIVATE METHODS
        private void CrossFadeAlpha(float targetAlpha, float duration)
        {
            if (graphicsToFade.Count == 0) return;

            foreach (MaskableGraphic graphic in graphicsToFade)
            {
                if (graphic)
                {
                    graphic.CrossFadeAlpha(targetAlpha, duration, true);
                }
            }
        }

        private void TweeningFade(float from, float to, float duration)
        {
            if (graphicsToFade.Count == 0) return;

            foreach (MaskableGraphic graphic in graphicsToFade)
            {
                if (graphic)
                {
                    graphic.DOFade(to, duration).From(from);
                }
            }
        }
        #endregion

        #region PUBLIC METHODS
        public void SetAlpha(float alpha)
        {
            if (graphicsToFade.Count == 0) return;

            foreach (MaskableGraphic graphic in graphicsToFade)
            {
                if (graphic)
                {
                    graphic.canvasRenderer.SetAlpha(alpha);
                }
            }
        }

        public void SetFadeDuration(float fadeDuration)
        {
            fadeOnDuration = fadeDuration;
        }

        public void FadeOff()
        {
            SetAlpha(SOLID_APLHA);
            CrossFadeAlpha(CLEAR_ALPHA, fadeOffDuration);
        }

        public void FadeOff(float fadeDuration)
        {
            //SetAlpha(SOLID_APLHA);
            //CrossFadeAlpha(CLEAR_ALPHA, fadeDuration);
            TweeningFade(SOLID_APLHA, CLEAR_ALPHA, fadeDuration);
        }

        public void FadeOn()
        {
            SetAlpha(CLEAR_ALPHA);
            CrossFadeAlpha(SOLID_APLHA, fadeOnDuration);
        }

        public void FadeOn(float fadeDuration)
        {
            SetAlpha(CLEAR_ALPHA);
            CrossFadeAlpha(SOLID_APLHA, fadeDuration);
        }
        #endregion
    }
}