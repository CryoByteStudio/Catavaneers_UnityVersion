using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Catavaneer.LevelManagement;
using ViTiet.Utils;

namespace Catavaneer.MenuSystem
{
    [RequireComponent(typeof(ScreenFader))]
    public class SplashScreen : MonoBehaviour
    {
        [SerializeField] protected float loadDelay = 0f;
        [SerializeField] protected float fadeDelay = 0f;
        [SerializeField] protected Slider loadingBar;
        [SerializeField] protected GameObject textObject;

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
            yield return new WaitForSeconds(loadDelay);
            LevelLoader.LoadNextLevelAsync(this);
            yield return new WaitForSeconds(fadeDelay);
            screenFader.FadeOff();
            Destroy(gameObject, screenFader.FadeOffDuration);
        }

        private void FadeOn()
        {
            StartCoroutine(FadeOnRoutine());
        }

        private IEnumerator FadeOnRoutine()
        {
            textObject?.SetActive(false);
            canvasGroup.interactable = false;
            screenFader.FadeOn();
            yield return new WaitForSeconds(screenFader.FadeOnDuration);
            canvasGroup.interactable = true;
            textObject?.SetActive(true);
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