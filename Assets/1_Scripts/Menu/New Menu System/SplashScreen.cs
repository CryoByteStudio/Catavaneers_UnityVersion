using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Catavaneer.LevelManagement;
using Catavaneer.Utils;

namespace Catavaneer.MenuSystem
{
    [RequireComponent(typeof(ScreenFader))]
    public class SplashScreen : MonoBehaviour
    {
        [SerializeField] protected float delay = 0f;
        [SerializeField] protected Slider loadingBar;

        protected ScreenFader screenFader;
        protected CanvasGroup canvasGroup;

        #region UNITY ENGINE FUNCTION

        virtual protected void Awake()
        {
            screenFader = GetComponent<ScreenFader>();
            if (!screenFader) EditorHelper.LogError(this, "No ScreenFader component found!");

            canvasGroup = GetComponent<CanvasGroup>();
            if (!screenFader) EditorHelper.LogError(this, "No Canvas component found!");
        }

        virtual protected void Start()
        {
            FadeOn();
        }

        virtual protected void Update()
        {
            loadingBar.value = LevelLoader.LoadingProgress;
        }

        #endregion

        #region PRIVATE METHODS

        private IEnumerator FadeAndLoadRoutine()
        {
            canvasGroup.interactable = false;
            LevelLoader.LoadNextLevelAsync(this);
            yield return new WaitForSeconds(delay);
            screenFader.FadeOff();
            Destroy(gameObject, screenFader.FadeOffDuration);
        }

        private void FadeOn()
        {
            StartCoroutine(FadeOnRoutine());
        }

        private IEnumerator FadeOnRoutine()
        {
            canvasGroup.interactable = false;
            screenFader.FadeOn();
            yield return new WaitForSeconds(screenFader.FadeOnDuration);
            canvasGroup.interactable = true;
        }

        #endregion

        #region PUBLIC METHODS

        public void FadeAndLoad()
        {
            StartCoroutine(FadeAndLoadRoutine());
        }

        #endregion
    }
}