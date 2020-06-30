using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Catavaneer.Extensions;

public class CreditAnimation : MonoBehaviour
{
    [SerializeField] private float moveDistance;
    [SerializeField] private float duration;
    [SerializeField] private float delay;
    [SerializeField] private List<MaskableGraphic> graphics;
    [SerializeField] private MaskableGraphic endingText;

    private void OnEnable()
    {
        foreach (var graphic in graphics)
        {
            graphic.DOFade(1, delay).From(0);
        }

        transform.DOMoveY(moveDistance, duration).SetEase(Ease.Linear).SetDelay(delay);
        endingText.DOFade(1, 1).SetDelay(duration);
    }
}
