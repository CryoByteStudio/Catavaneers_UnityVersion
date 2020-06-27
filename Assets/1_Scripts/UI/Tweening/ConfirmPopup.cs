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
    [SerializeField] private string label;
    [SerializeField] private TMP_Text labelField;
    [SerializeField] private Color labellColor;
    [Multiline]
    [SerializeField] private string description;
    [SerializeField] private TMP_Text descriptionField;
    [SerializeField] private Color descriptionlColor;
    [SerializeField] private string yesButtonText;
    [SerializeField] private TMP_Text yesButtonField;
    [SerializeField] private string noButtonText;
    [SerializeField] private TMP_Text noButtonField;
    [SerializeField] private Color textNormalColor;
    [SerializeField] private Color textSelectedColor;
    [SerializeField] private GameObject firstSelectButton;
    [SerializeField] private Ease inEaseType;
    [SerializeField] private Ease outEaseType;
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

        if (labelField) labelField.text = label;
        if (descriptionField) descriptionField.text = description;

        if (yesButtonField)
        {
            yesButtonField.text = yesButtonText;
            yesButtonField.color = textNormalColor;
        }

        if (noButtonField)
        {
            noButtonField.text = noButtonText;
            noButtonField.color = textNormalColor;
        }

        transform.localScale = Vector3.zero;
    }

    public void OnMoveToYes()
    {
        yesButtonField.color = textSelectedColor;
        noButtonField.color = textNormalColor;
    }

    public void OnMoveToNo()
    {
        noButtonField.color = textSelectedColor;
        yesButtonField.color = textNormalColor;
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
        transform.DOScale(1, animateDuration).SetEase(inEaseType).OnComplete(() => enableInput = true);
        isActivated = true;
        isFocused = true;
    }

    public void ClosePopup()
    {
        EventSystem.current.SetSelectedGameObject(null);
        transform.DOScale(0, animateDuration).SetEase(outEaseType).OnComplete(() => Reset());
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
}
