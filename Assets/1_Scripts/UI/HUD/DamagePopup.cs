using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using Catavaneer.Extensions;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private float animateDuration;
    [SerializeField] private float fadeDuration;
    [SerializeField] private float popHeight;
    [SerializeField] Ease easeType;
    [SerializeField] private MaskableGraphic graphicToFade;

    private void Start()
    {
        Play();
    }

    public void Play()
    {
        transform.DOMoveY(popHeight, animateDuration).SetRelative().SetEase(easeType).
            OnComplete (() =>
            {
                transform.DOMoveY(-popHeight, Mathf.Abs(animateDuration - fadeDuration)).SetRelative().SetEase(easeType);
                graphicToFade.DOFade(0, fadeDuration);
                Destroy(gameObject, Mathf.Max(animateDuration, fadeDuration));
            });
        transform.DOScale(3, animateDuration).SetEase(easeType);
    }
}
