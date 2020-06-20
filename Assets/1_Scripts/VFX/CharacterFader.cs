using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEditor;

public class CharacterFader : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer renderer;
    [SerializeField] private string materialPropertyName;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private float from;
    [SerializeField] private float to;
    [SerializeField] private bool swapMaterial;
    [SerializeField] private Material originalMaterial;
    [SerializeField] private Material newMaterial;

    private void Awake()
    {
        CreateMaterialInstance();
    }

    // Create new material instances
    private void CreateMaterialInstance()
    {
        renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        renderer.material = new Material(renderer.material);

        if (healthBarImage)
            healthBarImage.material = new Material(healthBarImage.material);
    }

    // Fade object model and health bar
    public void FadeOut(float duration)
    {
        ModelFadeOut(from, to, duration);
        HealthBarFadeOut(from, to, duration);
    }

    // Fade object model
    private void ModelFadeOut(float startValue, float endValue, float duration)
    {
        if (swapMaterial)
            renderer.material = new Material(newMaterial);

        renderer.material.SetFloat(materialPropertyName, startValue);
        renderer.material.DOFloat(endValue, materialPropertyName, duration);
    }

    // Fade health bar
    private void HealthBarFadeOut(float startValue, float endValue, float duration)
    {
        healthBarImage.material.SetFloat(materialPropertyName, startValue);
        healthBarImage.material.DOFloat(endValue, materialPropertyName, duration);
    }

    // Reset model and health bar
    public void ResetFade()
    {
        ResetModelFade(from);
        ResetHealthBarFade(from);
    }

    // Reset model
    private void ResetModelFade(float initialValue)
    {
        renderer.material.SetFloat(materialPropertyName, initialValue);

        if (swapMaterial)
            renderer.material = new Material(originalMaterial);
    }

    // Reset health bar
    private void ResetHealthBarFade(float initialValue)
    {
        healthBarImage.material.SetFloat(materialPropertyName, initialValue);
    }
}