using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    private bool isOpen = true;
    private HealthComp occupant = null;
    public Vector3 Position { get { return transform.position; } }

    public bool IsPointOpen()
    {
        return isOpen;
    }

    public void SetOccupant(HealthComp occupant)
    {
        this.occupant = occupant;

        // open if no occupant, close if has occupant
        SetOpen(occupant == null);
    }

    public void SetOpen(bool value)
    {
        isOpen = value;
    }
}
