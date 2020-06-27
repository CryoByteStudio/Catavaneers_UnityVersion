using UnityEngine;
using DG.Tweening;

public class Pointer : MonoBehaviour
{
    [SerializeField] private float moveDistance;
    [SerializeField] private float duration;
    [SerializeField] private Ease easeType;

    private void Start()
    {
        transform.DOMoveY(moveDistance, duration).SetRelative().SetLoops(-1, LoopType.Yoyo).SetEase(easeType);
    }

    private void OnValidate()
    {
        transform.DORestart();
    }
}
