using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PlayerFader : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer renderer;
    [SerializeField] private string materialPropertyName;
    [SerializeField] private Image healthBarImage;

    private void Awake()
    {
        CreateMaterialInstance();
    }

    private void CreateMaterialInstance()
    {
        renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        renderer.material = new Material(renderer.material);

        if (healthBarImage)
            healthBarImage.material = new Material(healthBarImage.material);
    }

    public void FadeOut(float startValue, float endValue, float duration)
    {
        ModelFadeOut(startValue, endValue, duration);
        HealthBarFadeOut(startValue, endValue, duration);
    }

    private void HealthBarFadeOut(float startValue, float endValue, float duration)
    {
        healthBarImage.material.SetFloat(materialPropertyName, startValue);
        healthBarImage.material.DOFloat(endValue, materialPropertyName, duration);
    }

    private void ModelFadeOut(float startValue, float endValue, float duration)
    {
        renderer.material.SetFloat(materialPropertyName, startValue);
        renderer.material.DOFloat(endValue, materialPropertyName, duration);
    }

    public void ResetFade(float initialValue)
    {
        ResetModelFade(initialValue);
        ResetHealthBarFade(initialValue);
    }

    private void ResetModelFade(float initialValue)
    {
        renderer.material.SetFloat(materialPropertyName, initialValue);
    }

    private void ResetHealthBarFade(float initialValue)
    {
        healthBarImage.material.SetFloat(materialPropertyName, initialValue);
    }
}
