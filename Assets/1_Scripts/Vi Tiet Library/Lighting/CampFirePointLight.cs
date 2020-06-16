using CustomMathLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Type = CustomMathLibrary.Interpolation.Easing.Type;

public class CampFirePointLight : MonoBehaviour
{
    [SerializeField] private Light pointLight;
    [SerializeField] private bool flicker;
    [Range(0f, 0.9f)]
    [SerializeField] private float minIntensity;
    [Range(0.1f, 1f)]
    [SerializeField] private float maxIntensity;
    [SerializeField] private Type mode;
    [Range(0f, 5f)]
    [SerializeField] private float minFlickerSpeed;
    [Range(0f, 5f)]
    [SerializeField] private float maxFlickerSpeed;
    
    private float flickerSpeed;
    private float lerpValue = 0f;
    private float step = 0f;
    private bool isZeroToOne = true;
    private int index = 0;
    private int nextIndex = 0;

    private void Start()
    {
        if (!pointLight)
            pointLight = GetComponent<Light>();

        RandomizeFlickerSpeed();
    }

    private void Update()
    {
        Flicker();
    }

    private void Flicker()
    {
        if (!flicker) return;

        UpdateStep(flickerSpeed);
        CalculateLerpValue();
        LerpLightIntensity();
        
        if (ToggleBoolean(ref isZeroToOne, step == minIntensity || step == maxIntensity))
        {
            RandomizeFlickerSpeed();
        }
    }

    private void LerpLightIntensity()
    {
        float intensityPreClamp;
        pointLight.intensity = CustomMathf.CalculateLerpValue(lerpValue, mode, isZeroToOne);
        intensityPreClamp = pointLight.intensity;
        pointLight.intensity = Mathf.Clamp(pointLight.intensity, minIntensity, maxIntensity);
    }

    private void CalculateLerpValue()
    {
        lerpValue = CustomMathf.CalculateLerpValueClamp01(step, mode, isZeroToOne);
    }

    private void UpdateStep(float speed)
    {
        step = isZeroToOne ? step + Time.deltaTime * speed : step - Time.deltaTime * speed;
        step = CustomMathf.ClampMinMax(minIntensity, maxIntensity, step);
    }

    private void RandomizeFlickerSpeed()
    {
        flickerSpeed = Random.Range(minFlickerSpeed, maxFlickerSpeed);
    }

    private static bool ToggleBoolean(ref bool boolean, bool toggleCondition)
    {
        boolean = toggleCondition ? !boolean : boolean;
        return toggleCondition;
    }

    private void OnValidate()
    {
        if (maxFlickerSpeed < minFlickerSpeed)
            maxFlickerSpeed = minFlickerSpeed + 0.1f;

        if (maxIntensity < minIntensity)
            maxIntensity = minIntensity + 0.1f;
    }
}
