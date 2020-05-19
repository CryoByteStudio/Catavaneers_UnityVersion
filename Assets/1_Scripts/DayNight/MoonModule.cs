using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonModule : DNMouduleBase
{
    [SerializeField] private Light Moon;
    [SerializeField] Gradient MoonColor;
    [SerializeField] private float BaseIntensity;

    public override void UpdateModule(float intensity)
    {
        Moon.color = MoonColor.Evaluate(1 - intensity);
        Moon.intensity = (1 - intensity * BaseIntensity + 0.05f);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
