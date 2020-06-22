using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private float animateDuration;
    [SerializeField] private float popHeight;
    [SerializeField] Ease easeType;
    [SerializeField] private MaskableGraphic graphicToFade;
    [SerializeField] private TMP_Text textField;
    [SerializeField] private Color textColor;

    private float fadeDuration;

    public void Play(int damage)
    {
        textField.text = "" + damage;
        textField.color = textColor;
        fadeDuration = animateDuration / 3f;
        transform.DOScale(3, animateDuration).SetEase(easeType);
        transform.DOMoveY(popHeight, animateDuration - fadeDuration).SetRelative().SetEase(easeType).
            OnComplete (() =>
            {
                graphicToFade.DOFade(0, fadeDuration);
                Destroy(gameObject, fadeDuration + 0.1f);
            });
    }
}