using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Catavaneer.LevelManagement;
using ViTiet.Utils;
using System.Collections.Generic;
using Catavaneer.Extensions;

namespace Catavaneer.MenuSystem
{
    public class TransitionFader : ScreenFader
    {
        [System.Serializable]
        public struct ImageToSwapData
        {
            public Sprite imageToSwap;
            public IntendedLevelImage intendedFor;
        }

        [SerializeField] protected TransitionFaderType type = TransitionFaderType.MainMenuTransition;
        [SerializeField] protected float displayDuration;
        [SerializeField] protected TMP_Text transitionTextField;
        [SerializeField] protected Slider loadingBar;
        [SerializeField] private Image targetImage;
        [SerializeField] private List<ImageToSwapData> imageToSwapDataList;

        public TransitionFaderType Type { get { return type; } }
        public List<ImageToSwapData> ImageToSwapDataList { get { return imageToSwapDataList; } }
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

        private void ChangeDisplayImage(IntendedLevelImage intendedLevel)
        {
            foreach (ImageToSwapData data in imageToSwapDataList)
            {
                if (data.intendedFor == intendedLevel)
                {
                    targetImage.sprite = data.imageToSwap;
                    break;
                }
            }
        }

        private void Play()
        {
            StartCoroutine(PlayRoutine());
        }

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

        public void SetTransitionText(string text)
        {
            if (transitionTextField)
                transitionTextField.text = text;
        }

        public void PlayTransition(int levelIndex, TransitionFader transitionFader)
        {
            if (!transitionFader)
            {
                EditorHelper.ArgumentNullException("transitionFader");
                return;
            }

            IntendedLevelImage intendedLevel = levelIndex.ToEnum<IntendedLevelImage>();

            if (intendedLevel != IntendedLevelImage.Default)
                ChangeDisplayImage(intendedLevel);

            TransitionFader instance = Instantiate(transitionFader, Vector3.zero, Quaternion.identity);
            instance.Play();
        }

        public void PlayTransition(string levelName, TransitionFader transitionFader)
        {
            if (!transitionFader)
            {
                EditorHelper.ArgumentNullException("transitionFader");
                return;
            }

            IntendedLevelImage intendedLevel = levelName.ToEnum<IntendedLevelImage>();

            if (intendedLevel != IntendedLevelImage.Default)
                ChangeDisplayImage(intendedLevel);

            TransitionFader instance = Instantiate(transitionFader, Vector3.zero, Quaternion.identity);
            instance.Play();
        }

        #endregion

        #region PRIVATE STATIC METHODS

        private static Sprite GetSwapSprite(IntendedLevelImage intendedLevel, List<ImageToSwapData> imageToSwapDataList)
        {
            foreach (ImageToSwapData data in imageToSwapDataList)
            {
                if (data.intendedFor == intendedLevel)
                {
                    return data.imageToSwap;
                }
            }

            return null;
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
