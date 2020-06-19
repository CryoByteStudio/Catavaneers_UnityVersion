using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] Weapon[] weapons = null;
    [SerializeField] float[] spawnChance = null;
    [SerializeField] Weapon currentWeapon = null;
    [SerializeField] Transform rightHandTransform = null;
    [SerializeField] Transform leftHandTransform = null;
    void Start()
    {
        float randomWeapon = Random.Range(0,100);
        if (currentWeapon == null)
        {
            if (randomWeapon >= spawnChance[2])         currentWeapon = weapons[3];
            else if (randomWeapon >= spawnChance[1])    currentWeapon = weapons[2];
            else if (randomWeapon >= spawnChance[0])    currentWeapon = weapons[1];
            else                                        currentWeapon = weapons[0];
        }
        EquipWeapon(currentWeapon);
    }
    public void EquipWeapon(Weapon weapon)
    {
        currentWeapon = weapon;
        Animator animator = GetComponent<Animator>();
        weapon.Spawn(rightHandTransform, leftHandTransform, animator);
    }
}
