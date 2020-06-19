using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] Weapon defaultWeapon = null;
    [SerializeField] Weapon currentWeapon = null;
    [SerializeField] Transform rightHandTransform = null;
    [SerializeField] Transform leftHandTransform = null;
    void Start()
    {
        if (currentWeapon == null)
        {
            EquipWeapon(defaultWeapon);
        }
    }
    public void EquipWeapon(Weapon weapon)
    {
        currentWeapon = weapon;
        GetComponent<PlayerController>().SetWeaponWeight(currentWeapon.GetWeaponWeight());
        Animator animator = GetComponent<Animator>();
        weapon.Spawn(rightHandTransform, leftHandTransform, animator);
    }
}
