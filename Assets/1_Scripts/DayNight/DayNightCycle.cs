using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct TimeOfDay
{
    public int Hour;
    public int Minute;
    public int Second;
}

public class DayNightCycle : MonoBehaviour
{
    [Header("Time")]
    [Tooltip("Day Length in Minutes")]

    [SerializeField] private float _TargetDayLength = 0.5f;
    public float TargetDayLength { get { return _TargetDayLength; } }

    [SerializeField] [Range(0f, 1f)] private float _TimeOfDay;
    public Vector2 _TimeOfDayInHours;
    public float TimeOfay { get { return _TimeOfDay; } }
    private float _timeScale = 100f;
    private bool Pause = false;

    [Header("Sun Light")]
    [SerializeField] private Transform DailyRotation;
    [SerializeField] private Light Sun;
    private float Intensity;
    [SerializeField] private float SunBaseIntensity = 1f;
    [SerializeField] private float SunVariation = 1.5f;
    [SerializeField] private Gradient SunColor;

    [Header("Modules")]
    private List<DNMouduleBase> ModuleList = new List<DNMouduleBase>();

    private void UpdateTimeScale()
    {
        _timeScale = 24 / (_TargetDayLength / 60);
    }

    private void UpdateTime()
    {
        _TimeOfDay += Time.deltaTime * _timeScale / 86400;
        _TimeOfDay %= 1;
    }

    private void Update()
    {
        if(!Pause)
        {
            UpdateTimeScale();
            UpdateTime();
        }

        CalculateTimeOfDayInHours();
        AdjustSunRotation();
        SunIntensity();
        UpdateModules();
    }

    private void CalculateTimeOfDayInHours()
    {
        _TimeOfDayInHours.x = (int)(_TimeOfDay * _timeScale / 60);
        _TimeOfDayInHours.y = (int)(_TimeOfDay * _timeScale % 60);
    }

    private void AdjustSunRotation()
    {
        float SunAngle = TimeOfay * 360f;
        DailyRotation.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, SunAngle));
    }

    private void SunIntensity()
    {
        Intensity = Vector3.Dot(Sun.transform.forward, Vector3.down);
        Intensity = Mathf.Clamp01(Intensity);

        Sun.intensity = Intensity * SunVariation + SunBaseIntensity;
    }

    private void AdjustSunColor()
    {
        Sun.color = SunColor.Evaluate(Intensity);
    }

    public void AddModule(DNMouduleBase module)
    {
        ModuleList.Add(module);
    }

    public void RemoveModule(DNMouduleBase module)
    {
        ModuleList.Remove(module);
    }

    private void UpdateModules()
    {
        foreach (DNMouduleBase module in ModuleList)
        {
            module.UpdateModule(Intensity);
        }
    }
}
