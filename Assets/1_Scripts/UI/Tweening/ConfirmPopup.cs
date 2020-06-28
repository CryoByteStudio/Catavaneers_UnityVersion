using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Catavaneer;
using DG.Tweening;

public class ConfirmPopup : MonoBehaviour
{
    [Header("Panel Properties")]
    [SerializeField] private Image panelBackground;
    [SerializeField] private Color panelBackgroundColor;

    [Header("Label Properties")]
    [SerializeField] private string labelText;
    [SerializeField] private TMP_Text labelTextField;
    [SerializeField] private Color labelTextColor;

    [Header("Description Properties")]
    [Multiline]
    [SerializeField] private string descriptionText;
    [SerializeField] private TMP_Text descriptionTextField;
    [SerializeField] private Color descriptionTextColor;

    [Header("Positive Button Properties")]
    [SerializeField] private string positiveButtonText;
    [SerializeField] private TMP_Text positiveButtonTextField;
    [SerializeField] private Image positiveButtonBackground;

    [Header("Negative Button Properties")]
    [SerializeField] private string negativeButtonText;
    [SerializeField] private TMP_Text negativeButtonTextField;
    [SerializeField] private Image negativeButtonBackground;

    [Header("Button Settings")]
    [SerializeField] private Color buttonTextNormalColor;
    [SerializeField] private Color buttonTextSelectedColor;
    [SerializeField] private Color buttonNormalColor;
    [SerializeField] private Color buttonSelectedColor;
    [SerializeField] private GameObject firstSelectButton;

    [Header("Animation Settings")]
    [SerializeField] private Ease outEaseType;
    [SerializeField] private Ease inEaseType;
    [SerializeField] private float animateDuration;

    private bool isActivated;
    public bool IsActivated => isActivated;
    private bool isFocused;
    public bool IsFocused => isFocused;
    private bool hasConfirmed;
    public bool HasConfirmed => hasConfirmed;
    private bool enableInput;
    public bool EnableInput => enableInput;

    private bool proceed;
    public bool Proceed => proceed;

    public float AnimateDuration => animateDuration;

    public delegate void ConfirmCallback();

    private void Start()
    {
        Reset();
    }

    private void Reset()
    {
        isActivated = false;
        isFocused = false;
        hasConfirmed = false;
        proceed = false;

        if (labelTextField) labelTextField.text = labelText;
        if (descriptionTextField) descriptionTextField.text = descriptionText;

        if (positiveButtonTextField)
        {
            positiveButtonTextField.text = positiveButtonText;
            positiveButtonTextField.color = buttonTextNormalColor;
        }

        if (negativeButtonTextField)
        {
            negativeButtonTextField.text = negativeButtonText;
            negativeButtonTextField.color = buttonTextNormalColor;
        }

        transform.localScale = Vector3.zero;
    }

    public void OnMoveToYes()
    {
        positiveButtonTextField.color = buttonTextSelectedColor;
        positiveButtonBackground.color = buttonSelectedColor;
        negativeButtonTextField.color = buttonTextNormalColor;
        negativeButtonBackground.color = buttonNormalColor;
    }

    public void OnMoveToNo()
    {
        negativeButtonTextField.color = buttonTextSelectedColor;
        negativeButtonBackground.color = buttonSelectedColor;
        positiveButtonTextField.color = buttonTextNormalColor;
        positiveButtonBackground.color = buttonNormalColor;
    }

    public void OnYesPressed()
    {
        hasConfirmed = true;
        proceed = true;
    }

    public void OnNoPressed()
    {
        hasConfirmed = true;
        proceed = false;
    }

    public void OpenPopup()
    {
        EventSystem.current.SetSelectedGameObject(firstSelectButton);
        transform.DOScale(1, animateDuration).SetEase(outEaseType).OnComplete(() => enableInput = true);
        isActivated = true;
        isFocused = true;
    }

    public void ClosePopup()
    {
        EventSystem.current.SetSelectedGameObject(null);
        transform.DOScale(0, animateDuration).SetEase(inEaseType).OnComplete(() => Reset());
    }

    public void ExecuteOnConfirm(ConfirmCallback yesAction, ConfirmCallback noAction)
    {
        if (!isActivated)
            StartCoroutine(WaitForConfirmation(yesAction, noAction));
    }

    private IEnumerator WaitForConfirmation(ConfirmCallback yesAction, ConfirmCallback noAction)
    {
        OpenPopup();
        yield return new WaitForSeconds(animateDuration + 0.1f);

        while (isActivated)
        {
            if (hasConfirmed)
            {
                if (proceed)
                {
                    if (yesAction != null) yesAction.Invoke();
                }
                else if (noAction != null) noAction.Invoke();

                ClosePopup();
                isActivated = false;
            }

            yield return null;
        }
    }

    private void OnValidate()
    {
        if (panelBackground) panelBackground.color = panelBackgroundColor;
        if (labelTextField)
        {
            labelTextField.text = labelText;
            labelTextField.color = labelTextColor;
        }
        if (descriptionTextField)
        {
            descriptionTextField.text = descriptionText;
            descriptionTextField.color = descriptionTextColor;
        }
        if (positiveButtonTextField)
        {
            positiveButtonTextField.text = positiveButtonText;
            positiveButtonTextField.color = buttonTextNormalColor;
        }
        if (negativeButtonTextField)
        {
            negativeButtonTextField.text = negativeButtonText;
            negativeButtonTextField.color = buttonTextNormalColor;
        }
        if (positiveButtonBackground) positiveButtonBackground.color = buttonNormalColor;
        if (negativeButtonBackground) negativeButtonBackground.color = buttonNormalColor;
    }
}
