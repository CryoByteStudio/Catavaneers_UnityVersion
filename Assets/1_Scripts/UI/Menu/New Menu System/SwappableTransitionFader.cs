using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Catavaneer.Extensions;

namespace Catavaneer.MenuSystem
{
    public class SwappableTransitionFader : TransitionFader
    {
        [System.Serializable]
        public struct ImageToSwapData
        {
            public Sprite imageToSwap;
            public IntendedLevelImage intendedFor;
        }

        [SerializeField] private Image targetImage;
        [SerializeField] private List<ImageToSwapData> imageToSwapDataList;

        public void PlayTransition(int levelIndex, TransitionFader transitionFader)
        {
            IntendedLevelImage intendedLevel = levelIndex.ToEnum<IntendedLevelImage>();

            if (intendedLevel != IntendedLevelImage.Default)
                ChangeDisplayImage(intendedLevel);

            PlayTransition(transitionFader);
        }

        public void PlayTransition(string levelName, TransitionFader transitionFader)
        {
            IntendedLevelImage intendedLevel = levelName.ToEnum<IntendedLevelImage>();

            if (intendedLevel != IntendedLevelImage.Default)
                ChangeDisplayImage(intendedLevel);

            PlayTransition(transitionFader);
        }

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
    }
}