using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    [SerializeField] Weapon weapon = null;
    [SerializeField] GameObject DropItem;
    Weapon PlayerOldWeapon;
    bool CanChangeWeapon;

    public Weapon GetThisWeapon()
    {
        return weapon;
    }
}

