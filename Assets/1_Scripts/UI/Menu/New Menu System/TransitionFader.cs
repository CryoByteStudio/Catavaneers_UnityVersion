using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Catavaneer.LevelManagement;
using ViTiet.Utils;

namespace Catavaneer.MenuSystem
{
    public class TransitionFader : ScreenFader
    {
        [SerializeField] private TransitionFaderType type = TransitionFaderType.MainMenuTransition;
        [SerializeField] private float displayDuration;
        [SerializeField] private TMP_Text transitionTextField;
        [SerializeField] private Slider loadingBar;

        public TransitionFaderType Type { get { return type; } }
        public float DisplayDuration { get { return displayDuration; } }
        public float LifeTime { get { return FadeOnDuration + displayDuration + FadeOffDuration; } }

        #region UNITY ENGINE FUNCTION

        protected override void Awake()
        {
            base.Awake();

            if (!transitionTextField)
            {
                transitionTextField = GetComponentInChildren<TMP_Text>();
            }

            if (!loadingBar)
            {
                loadingBar = GetComponentInChildren<Slider>();
            }
        }

        private void Update()
        {
            if (loadingBar && LevelLoader.IsLoading)
            {
                loadingBar.value = LevelLoader.LoadingProgress;
            }
        }

        #endregion

        #region PRIVATE METHODS

        private IEnumerator PlayRoutine()
        {
            FadeOn();
            yield return new WaitForSeconds(FadeOnDuration + displayDuration);

            if (loadingBar)
                yield return LoadingRoutine();

            FadeOff(FadeOffDuration);
            Destroy(gameObject, FadeOffDuration);
        }

        private IEnumerator LoadingRoutine()
        {
            while (loadingBar && LevelLoader.IsLoading)
            {
                loadingBar.value = LevelLoader.LoadingProgress;
                yield return new WaitForEndOfFrame();
            }
        }

        #endregion

        #region PUBLIC METHODS

        public void Play()
        {
            StartCoroutine(PlayRoutine());
        }

        public void SetTransitionText(string text)
        {
            transitionTextField.text = text;
        }

        #endregion

        #region PUBLIC STATIC METHODS

        public static void PlayTransition(TransitionFader transitionFader)
        {
            if (!transitionFader)
            {
                EditorHelper.ArgumentNullException("transitionFader");
                return;
            }

            TransitionFader instance = Instantiate(transitionFader, Vector3.zero, Quaternion.identity);
            instance.Play();
        }

        #endregion
    }
}
