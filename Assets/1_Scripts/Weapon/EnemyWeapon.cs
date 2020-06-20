using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] Weapon[] weapons = null;
    [SerializeField] Weapon currentWeapon = null;
    [SerializeField] Transform rightHandTransform = null;
    [SerializeField] Transform leftHandTransform = null;

    public Weapon CurrentWeapon => currentWeapon;

    public void Init()
    {
        SpawnWeapon();
        EquipWeapon(currentWeapon);
    }

    private void SpawnWeapon()
    {
        int randomInt = Random.Range(0, weapons.Length);
        currentWeapon = weapons[randomInt];
    }

    public void EquipWeapon(Weapon weapon)
    {
        currentWeapon = weapon;
        Animator animator = GetComponent<Animator>();
        weapon.Spawn(rightHandTransform, leftHandTransform, animator);
    }
}
