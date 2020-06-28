using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSwap : MonoBehaviour
{
    [SerializeField] private bool canSwap;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite swapSprite;
    [SerializeField] private float interval;
    [SerializeField] private Image image;

    private float timeToSwap = Mathf.Infinity;
    private bool isDefault;
    public bool CanSwap => canSwap;

    private void Start()
    {
        Init();
    }

    private void OnEnable()
    {
        Init();
    }

    private void OnDisable()
    {
        Reset();
    }

    private void Init()
    {
        if (!image)
            image = GetComponent<Image>();
        else
        {
            Reset();

            if (canSwap)
                StartSwappingSprite();
        }
    }

    private void Reset()
    {
        image.sprite = defaultSprite;
        isDefault = true;
    }

    public void SetCanSwap(bool value, bool startPlaying = true)
    {
        canSwap = value;

        if (canSwap && startPlaying)
            StartSwappingSprite();
    }

    public void StartSwappingSprite()
    {
        StartCoroutine(SwapSpriteRoutine());
    }

    private void SwapSprite()
    {
        image.sprite = isDefault ? swapSprite : defaultSprite;
        isDefault = !isDefault;
    }

    private IEnumerator SwapSpriteRoutine()
    {
        timeToSwap = Time.time + interval;
        while (canSwap)
        {
            if (Time.time >= timeToSwap)
            {
                SwapSprite();
                timeToSwap = Time.time + interval;
            }

            yield return null;
        }
    }
}