﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    [SerializeField] float CharacterAttackSpeed = 0.0f;
    [SerializeField] int CharacterAttackDamage = 0;
    [SerializeField] Weapon defaultWeapon = null;
    [SerializeField] GameObject[] weaponColliders;
    [SerializeField] Transform rightHandTransform = null;
    [SerializeField] Transform leftHandTransform = null;
    [SerializeField] Weapon currentWeapon;
    [SerializeField] BoxCollider currentWeaponCollider;
    [SerializeField] Transform projectileSpwanPoint;

    HealthComp target;
    float timeSinceLastAttack = Mathf.Infinity;
    PlayerController player;

    public string attackAxis;
    public string dodgeButton;
    void Start()
    {
        if (currentWeapon == null)
        {
            EquipWeapon(defaultWeapon);
        }
        else
        {
            EquipWeapon(currentWeapon);
        }
        player = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastAttack += Time.deltaTime;
        if (player.GetMoveState() == PlayerController.MoveStates.Freeze ) return;
        if(Input.GetAxis(attackAxis) >0 && timeSinceLastAttack > GetCurrentAttackSpeed())
        {
            timeSinceLastAttack = 0;
            GetComponent<Animator>().SetTrigger("Attack");
            ShootProjectile();
            
        }

        //if(Input.GetButtonDown("Attack"))
        //{
        //    ShootProjectile();
        //}

        if (Input.GetButtonDown(dodgeButton))
        {
            GetComponent<Animator>().SetTrigger("Roll");
        }
    }
    public void EquipWeapon(Weapon weapon)
    {
        currentWeapon = weapon;
        if(currentWeaponCollider != null) currentWeaponCollider.gameObject.SetActive(false);
        GetComponent<PlayerController>().SetWeaponWeight(currentWeapon.GetWeaponWeight());
        Animator animator = GetComponent<Animator>();
        weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        //Debug.Log(currentWeapon.name);
        foreach(GameObject weaponCollider in weaponColliders)
        {
            if (weaponCollider.name == currentWeapon.name)
            {
                Debug.Log(weaponCollider.name);
                weaponCollider.gameObject.SetActive(true);
                //weaponCollider.GetComponent<BoxCollider>().enabled = true;
                currentWeaponCollider = weaponCollider.GetComponent<BoxCollider>();
            }
        }
    }
    //old attack system from animation event
    //void Hit()
    //{
    //    Debug.Log("attack called");
    //    float halfRaycastLength = currentWeapon.GetWeaponRange();
    //    //rayStart.position = attackRayOrigin.position - new Vector3(halfRaycastLength, 0, 0);
    //    //rayEnd.position = attackRayOrigin.position + new Vector3(halfRaycastLength, 0, 0);
    //    Vector3 raycastDirection = rayEnd.transform.position - rayStart.transform.position;
    //    float rayDistance = Vector3.Distance(rayStart.position, rayEnd.position);
    //    RaycastHit[] hits = Physics.RaycastAll(rayStart.position, raycastDirection, rayDistance);
    //    Debug.DrawRay(rayStart.position, raycastDirection, Color.red, 2f);

    //    foreach (RaycastHit hit in hits)
    //    {
    //        Debug.Log("hit = " + hit.transform.name);
    //        target = hit.transform.GetComponent<HealthComp>();
    //        if (target != null)
    //        {
    //            target.TakeDamage(GetCurrentAttackDamage());
    //            Debug.Log("object name: " + hit.transform.name + " takes damage");
    //        }
    //        else
    //        {
    //            Debug.Log("object name: " + hit.transform.name + "is not targetable");
    //        }
    //    }
    //}
    //public void UpdateRaycastOrientation()
    //{
    //    rayStart.transform.localPosition = new Vector3(currentWeapon.GetWeaponRange(),0,0);
    //    rayEnd.transform.localPosition = new Vector3(-currentWeapon.GetWeaponRange(),0,0);
    //}

    //end of old attack system from animation event
    float GetCurrentAttackSpeed()
    {
       // Debug.Log("character attack speed: " + CharacterAttackSpeed);
       // Debug.Log("weapon attack speed: " + currentWeapon.GetWeaponAttackSpeed());
       // Debug.Log("final attack speed: " + CharacterAttackSpeed * currentWeapon.GetWeaponAttackSpeed());
        return CharacterAttackSpeed * currentWeapon.GetWeaponAttackSpeed();
    }
    int GetCurrentAttackDamage()
    {
        return CharacterAttackDamage * currentWeapon.GetDamage();
    }
    public float GetWeaponWeight()
    {
        return currentWeapon.GetWeaponWeight();
    }
    void StartHit()
    {
        Debug.Log("its on");
        Debug.Log(currentWeaponCollider);
        currentWeaponCollider.GetComponent<BoxCollider>().enabled = true;
        //currentWeaponCollider.enabled = true;
    }
    void EndHit()
    {
        Debug.Log("its off");
        Debug.Log(currentWeaponCollider);
        // currentWeaponCollider.enabled = false;
        currentWeaponCollider.GetComponent<BoxCollider>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<HealthComp>().TakeDamage(currentWeapon.GetDamage());
        } else if (other.tag == "Player" && FindObjectOfType<GameDifficultyManager>().dif == DifficultyLevel.Catfight)
        {
            other.GetComponent<HealthComp>().TakeDamage(currentWeapon.GetDamage());
        }
    }

    public void ShootProjectile()
    {
        if (projectileSpwanPoint == null) { return; }

        if (currentWeapon.HasProjectile())
        {
            currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, projectileSpwanPoint);
        }
    }

}