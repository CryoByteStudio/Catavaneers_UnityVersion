using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    [SerializeField] float CharacterSpeed = 0.0f;
    [SerializeField] float CharacterAttackSpeed = 0.0f;
    [SerializeField] float CharacterAttackDamage = 0.0f;
    [SerializeField] Weapon defaultWeapon = null;
    [SerializeField] Transform rightHandTransform = null;
    [SerializeField] Transform leftHandTransform = null;
    [SerializeField] Weapon currentWeapon;
    [SerializeField] Transform attackRayOrigin = null;
    [SerializeField] Transform rayStart = null;
    [SerializeField] Transform rayEnd = null;

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
        UpdateRaycastOrientation();
        if (player.GetMoveState() == PlayerController.MoveStates.Freeze ) return;
        if(Input.GetAxis(attackAxis) >0 && timeSinceLastAttack > GetCurrentAttackSpeed())
        {
            timeSinceLastAttack = 0;
            GetComponent<Animator>().SetTrigger("Attack");
        }
        if (Input.GetButtonDown(dodgeButton))
        {
            GetComponent<Animator>().SetTrigger("Roll");
        }
    }

    public void EquipWeapon(Weapon weapon)
    {
        currentWeapon = weapon;
        GetComponent<PlayerController>().SetWeaponWeight(currentWeapon.GetWeaponWeight());
        Animator animator = GetComponent<Animator>();
        weapon.Spawn(rightHandTransform, leftHandTransform, animator);
    }
    //Animation Event (RPG-Character@Unarmed-Attack-L1)
    void Hit()
    {
        Debug.Log("attack called");
        float halfRaycastLength = currentWeapon.GetWeaponRange();
        //rayStart.position = attackRayOrigin.position - new Vector3(halfRaycastLength, 0, 0);
        //rayEnd.position = attackRayOrigin.position + new Vector3(halfRaycastLength, 0, 0);
        Vector3 raycastDirection = rayEnd.transform.position - rayStart.transform.position;
        float rayDistance = Vector3.Distance(rayStart.position, rayEnd.position);
        RaycastHit[] hits = Physics.RaycastAll(rayStart.position, raycastDirection, rayDistance);
        Debug.DrawRay(rayStart.position, raycastDirection, Color.red, 2f);

        foreach (RaycastHit hit in hits)
        {
            Debug.Log("hit = " + hit.transform.name);
            target = hit.transform.GetComponent<HealthComp>();
            if (target != null)
            {
                target.TakeDamage(currentWeapon.GetDamage());
                Debug.Log("object name: " + hit.transform.name + " takes damage");
            }
            else
            {
                Debug.Log("object name: " + hit.transform.name + "is not targetable");
            }
        }
    }
    public void UpdateRaycastOrientation()
    {
        rayStart.transform.localPosition = new Vector3(currentWeapon.GetWeaponRange(),0,0);
        rayEnd.transform.localPosition = new Vector3(-currentWeapon.GetWeaponRange(),0,0);
    }
    float GetCurrentAttackSpeed()
    {
        return CharacterAttackSpeed * currentWeapon.GetWeaponAttackSpeed();
    }
    float GetCurrentAttackDamage()
    {
        return CharacterAttackDamage * currentWeapon.GetDamage();
    }
    float GetCurrentCharacterSpeed()
    {
        return CharacterSpeed * currentWeapon.GetWeaponWeight();
    }
}
