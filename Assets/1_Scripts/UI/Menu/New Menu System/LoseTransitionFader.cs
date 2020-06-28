using Catavaneer.Singleton;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Catavaneer.Extensions;
using Catavaneer.MenuSystem;

public class LoseTransitionFader : SingletonEntity<LoseTransitionFader>
{
    [SerializeField] private float fadeOnDuration;
    [SerializeField] private float displayDuration;
    [SerializeField] private float fadeOffDuration;
    [SerializeField] private List<MaskableGraphic> graphics;

    public float FadeOnDuration => fadeOffDuration;
    public float DisplayDuration => displayDuration;
    public float FadeOffDuration => fadeOffDuration;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        if (graphics == null || graphics.Count <= 0)
            graphics = transform.GetAllComponentsOfTypeInHierachy<MaskableGraphic>();
    }

    private void Start()
    {
        //gameObject.SetActive(false);
    }

    public void Play()
    {
        foreach (var item in graphics)
        {
            item.DOFade(1, fadeOnDuration).From(0).OnComplete(() =>
            {
                MenuManager.OpenMenu(MenuManager.LoseMenu);
                item.DOFade(0, fadeOffDuration).SetDelay(displayDuration).OnComplete(()=> 
                {
                    gameObject.SetActive(false);
                });
            });
        }
    }

    private void OnValidate()
    {
        if (graphics == null || graphics.Count <= 0)
            graphics = transform.GetAllComponentsOfTypeInHierachy<MaskableGraphic>();
    }
}
