using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private float minAnimateDuration;
    [SerializeField] private float maxAnimateDuration;
    [SerializeField] private float popHeight;
    [SerializeField] private float minScale = 3;
    [SerializeField] private float maxScale = 5;
    [SerializeField] Ease easeType;
    [SerializeField] private MaskableGraphic graphicToFade;
    [SerializeField] private TMP_Text textField;
    [SerializeField] private Color textColor;

    private float animateDuration;
    private float fadeDuration;

    public void Play(int damage)
    {
        textField.text = "" + damage;
        textField.color = textColor;
        animateDuration = Random.Range(minAnimateDuration, maxAnimateDuration);
        fadeDuration = animateDuration / 3f;
        transform.DOScale(Random.Range(minScale, maxScale), animateDuration).SetEase(easeType);
        transform.DOMoveY(popHeight, animateDuration - fadeDuration).SetRelative().SetEase(easeType).
            OnComplete (() =>
            {
                graphicToFade.DOFade(0, fadeDuration);
                Destroy(gameObject, fadeDuration + 0.1f);
            });
    }

    public void Play(string damage)
    {
        textField.text = damage;
        textField.color = textColor;
        animateDuration = Random.Range(minAnimateDuration, maxAnimateDuration);
        fadeDuration = animateDuration / 3f;
        transform.DOScale(Random.Range(minScale, maxScale), animateDuration).SetEase(easeType);
        transform.DOMoveY(popHeight, animateDuration - fadeDuration).SetRelative().SetEase(easeType).
            OnComplete(() =>
            {
                graphicToFade.DOFade(0, fadeDuration);
                Destroy(gameObject, fadeDuration + 0.1f);
            });
    }
}