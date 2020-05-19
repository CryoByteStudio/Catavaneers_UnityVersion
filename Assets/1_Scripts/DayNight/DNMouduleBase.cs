using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DNMouduleBase : MonoBehaviour
{
    protected DayNightCycle DayNightControl;

    private void OnEnable()
    {
        DayNightControl = this.GetComponent<DayNightCycle>();
        if (DayNightControl != null)
            DayNightControl.AddModule(this);
    }

    private void OnDisable()
    {
        if (DayNightControl != null)
            DayNightControl.RemoveModule(this);
    }


    public abstract void UpdateModule(float intensity);

}
