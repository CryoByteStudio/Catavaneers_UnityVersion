using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxModule : DNMouduleBase
{
    [SerializeField] private Gradient SkyColor;
    [SerializeField] private Gradient HorizonColor;

    public override void UpdateModule(float intensity)
    {
        RenderSettings.skybox.SetColor("_SkyTint", SkyColor.Evaluate(intensity));
        RenderSettings.skybox.SetColor("_GrandColor", HorizonColor.Evaluate(intensity));
    }
    
}
