﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    private bool isOpen = true;
    private HealthComp occupant = null;
    public Vector3 Position { get { return transform.position; } }
    
    private void OnDisable()
    {
        if (occupant)
            occupant.OnEnemyDeath -= OnEnemyDeathHandler;
    }

    public bool IsPointOpen()
    {
        return isOpen = occupant == null;
    }

    public void SetOccupant(HealthComp occupant)
    {
        this.occupant = occupant;
        if (occupant)
            occupant.OnEnemyDeath += OnEnemyDeathHandler;
    }

    private void OnEnemyDeathHandler(HealthComp healthComp)
    {
        SetOccupant(null);
    }
}
