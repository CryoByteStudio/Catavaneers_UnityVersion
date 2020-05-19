using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script write by Will

public class WeaponSystem : MonoBehaviour
{
    //    [SerializeField] float WeaponDemage;
    //    [SerializeField] Transform RightHand = null;
    //    [SerializeField] Transform LeftHand = null;
    //    [SerializeField] Weapon CurrentWeapon = null;
    //    [SerializeField] float CurrentWeaponDamage;
    //    [SerializeField] float CurrentWeaponRange;
    //    [SerializeField] float CurrentAttackSpeed;
    //    [SerializeField] float CurrentWeight;
    //    [SerializeField] Transform ProjectileShootingPoint = null;
    //    [SerializeField] GameObject WeaponDropLocation = null;
    //    [SerializeField] GameObject DropWeapon = null;
    //    [SerializeField] Weapon WeaponThatisGoingToEquipt =null;
    //    [SerializeField] GameObject RaycastHitPoint =null;

    //    // Start is called before the first frame update
    //    void Start()
    //    {
    //        //EquipWeapon(CurrentWeapon);
    //    }

    //    // Update is called once per frame
    //    void Update()
    //    {
    //        if (Input.GetKeyDown(KeyCode.JoystickButton7))
    //        {
    //            Debug.Log("Menu bar shows up");
    //        }

    //        //if (Input.GetKeyDown(KeyCode.A))
    //        //{
    //        //    if (ProjectileShootingPoint == null) { return; }

    //        //    if (CurrentWeapon.HasProjectile())
    //        //    {
    //        //        CurrentWeapon.LaunchProjectile(RightHand, LeftHand, ProjectileShootingPoint);
    //        //    }
    //        //}

    //        //RaycastHit Hit;

    //        //if (Physics.Raycast(RaycastHitPoint.transform.position, transform.TransformDirection(Vector3.forward), out Hit, 1f, 1))
    //        //{
    //        //    Debug.DrawRay(RaycastHitPoint.transform.position, transform.TransformDirection(Vector3.forward) * Hit.distance, Color.yellow);
    //        //    Debug.Log("did hit");

    //        //    if (Hit.transform.gameObject.tag == "WeaponPickUP" && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton0)))
    //        //    {
    //        //        if (CurrentWeapon != null)
    //        //        {
    //        //            Instantiate(CurrentWeapon.GetDropitemPickUp(), transform.position, Quaternion.identity);
    //        //        }

    //        //        EquipWeapon(Hit.transform.GetComponent<WeaponPickUp>().GetThisWeapon());

    //        //        Destroy(Hit.transform.gameObject);
    //        //    }
    //        //}
    //        //else
    //        //{
    //        //    Debug.DrawRay(RaycastHitPoint.transform.position, transform.TransformDirection(Vector3.forward) * 1f, Color.white);
    //        //}

    //    }

    //    //public void EquipWeapon(Weapon weapon)
    //    //{

    //    //    if (CurrentWeapon != null)
    //    //    {
    //    //        CurrentWeapon = weapon;
    //    //    }
    //    //    //Animator animator = GetComponent<Animator>();

    //    //    weapon.Spawn(RightHand, LeftHand, animator);

    //    //    CurrentWeaponDamage = weapon.GetDamage();
    //    //    CurrentWeaponRange = weapon.GetWeaponRange();
    //    //    CurrentAttackSpeed = weapon.GetWeaponAttackSpeed();
    //    //    CurrentWeight = weapon.GetWeaponWeight();
    //    //}


    //    //private void OnTriggerStay(Collider other)
    //    //{
    //    //    if (other.gameObject.tag == "WeaponPickUP" && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton0)))
    //    //    {
    //    //        if (CurrentWeapon != null)
    //    //        {
    //    //            Instantiate(CurrentWeapon.GetDropitemPickUp(), transform.position, Quaternion.identity);
    //    //        }

    //    //        EquipWeapon(other.GetComponent<WeaponPickUp>().GetThisWeapon());

    //    //        Destroy(other.gameObject);
    //    //    }
    //    //}

    //    public Weapon GetCurrentWeapon()
    //    {
    //        return CurrentWeapon;
    //    }
}