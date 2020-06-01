using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShopTest : MonoBehaviour
{
    [SerializeField] Weapon Sword= null;
    [SerializeField] Weapon Axe = null;
    [SerializeField] Weapon Rapier = null;
    [SerializeField] Weapon Whip = null;
    [SerializeField] Weapon Hammer = null;
    [SerializeField] Weapon Unarmed = null;

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("shop active");
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                other.GetComponent<Fighter>().EquipWeapon(Sword);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                other.GetComponent<Fighter>().EquipWeapon(Axe);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                other.GetComponent<Fighter>().EquipWeapon(Rapier);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                other.GetComponent<Fighter>().EquipWeapon(Whip);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                other.GetComponent<Fighter>().EquipWeapon(Hammer);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                other.GetComponent<Fighter>().EquipWeapon(Unarmed);
            }
        }
    }
}
