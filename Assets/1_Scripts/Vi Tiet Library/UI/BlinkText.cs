using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CustomMathLibrary;
using Type = CustomMathLibrary.Interpolation.Easing.Type;

namespace ViTiet.UI
{
    public class BlinkText : MonoBehaviour
    {
        [SerializeField] private TMP_Text textField;
        [SerializeField] private bool blink;
        [SerializeField] private Type mode;
        [SerializeField] private float blinkSpeed;
        [SerializeField] private List<Color> blinkColors;

        private Color color;
        private float lerpValue = 0f;
        private float step = 0f;
        private bool isZeroToOne = true;
        private int index = 0;
        private int nextIndex = 0;

        #region UNITY ENGINE FUNCTION

        private void Start()
        {
            if (!textField)
                textField = GetComponent<TMP_Text>();
        }

        private void Update()
        {
            Blink();
        }

        #endregion

        #region PRIVATE METHODS

        private void Blink()
        {
            if (!blink) return;

            UpdateStep();
            ToggleBoolean(ref isZeroToOne, step == 0 || step == 1);
            CalculateLerpValue();
            ChangeTextColor();
        }

        private void ChangeTextColor()
        {
            nextIndex = CustomMathf.GetNextLoopIndex(index, blinkColors.Count);
            index = (step == 0 || step == 1) ? index = nextIndex : index;
            color = LerpColorLoop(index, blinkColors.Count, step, isZeroToOne);
            color.a = 1;
            textField.faceColor = color;
        }

        private Color LerpColorLoop(int currentIndex, int maxIndex, float step, bool isZeroToOne)
        {
            int nextIndex = CustomMathf.GetNextLoopIndex(currentIndex, maxIndex);

            if (isZeroToOne)
                return Color.Lerp(blinkColors[currentIndex], blinkColors[nextIndex], step);
            else
                return Color.Lerp(blinkColors[nextIndex], blinkColors[currentIndex], step);
        }

        private void CalculateLerpValue()
        {
            lerpValue = CustomMathf.CalculateLerpValueClamp01(step, mode, isZeroToOne);
        }

        private void UpdateStep()
        {
            step = isZeroToOne ? step + Time.deltaTime * blinkSpeed : step - Time.deltaTime * blinkSpeed;
            step = CustomMathf.ClampMinMax(0f, 1f, step);
        }

        #endregion

        #region PUBLIC METHODS

        public void ChangeText(string text)
        {
            this.textField.text = text;
        }

        #endregion

        #region PRIVATE STATIC METHODS

        private static void ToggleBoolean(ref bool boolean, bool toggleCondition)
        {
            boolean = toggleCondition ? !boolean : boolean;
        }

        #endregion
    }
}