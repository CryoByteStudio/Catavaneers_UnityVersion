using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class DisplayPopup: MonoBehaviour
{
    [SerializeField] private float minAnimateDuration;
    [SerializeField] private float maxAnimateDuration;
    [SerializeField] private float popHeight;
    [SerializeField] private float minScale = 3;
    [SerializeField] private float maxScale = 5;
    [Range(0f, 0.5f)]
    [SerializeField] private float fadeDurationPercentage;
    [SerializeField] Ease easeType;
    [SerializeField] private MaskableGraphic graphicToFade;
    [SerializeField] private TMP_Text textField;
    [SerializeField] private Color textColor;

    private float animateDuration;
    private float fadeDuration;
    private Vector3 startPosition;
    private string startText;

    private void Start()
    {
        startPosition = transform.position;
        startText = textField.text;
    }

    public void Play(int damage)
    {
        textField.text = "" + damage;
        textField.color = textColor;
        animateDuration = Random.Range(minAnimateDuration, maxAnimateDuration);
        fadeDuration = animateDuration * fadeDurationPercentage;
        transform.DOScale(Random.Range(minScale, maxScale), animateDuration).SetEase(easeType);
        transform.DOMoveY(popHeight, animateDuration - fadeDuration).SetRelative().SetEase(easeType).
            OnComplete(() =>
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
        fadeDuration = animateDuration * fadeDurationPercentage;
        transform.DOScale(Random.Range(minScale, maxScale), animateDuration).SetEase(easeType);
        transform.DOMoveY(popHeight, animateDuration - fadeDuration).SetRelative().SetEase(easeType).
            OnComplete(() =>
            {
                graphicToFade.DOFade(0, fadeDuration);
                Destroy(gameObject, fadeDuration + 0.1f);
            });
    }

    public void PlayTest()
    {
        textField.text = "Test";
        textField.color = textColor;
        animateDuration = Random.Range(minAnimateDuration, maxAnimateDuration);
        fadeDuration = animateDuration * fadeDurationPercentage;
        transform.DOScale(Random.Range(minScale, maxScale), animateDuration).SetEase(easeType);
        transform.DOMoveY(popHeight, animateDuration - fadeDuration).SetRelative().SetEase(easeType).
            OnComplete(() =>
            {
                graphicToFade.DOFade(0, fadeDuration);
            });
    }

    public void Reset()
    {
        transform.localScale = Vector3.one;
        transform.position = startPosition;
        graphicToFade.CrossFadeAlpha(1, 0, true);
        textField.text = startText;
    }

    private void OnValidate()
    {
        if (textField)
            textField.color = textColor;
    }
}