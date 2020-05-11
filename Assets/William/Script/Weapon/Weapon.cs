using UnityEngine;
//Script Write By Will


[CreateAssetMenu(fileName = "Weapon",menuName = "Weapon/Make New Weapon",order = 1)]

public class Weapon : ScriptableObject
{
    [SerializeField] GameObject EquippedPrefab = null;
    [SerializeField] AnimatorOverrideController animatorOverride = null;
    [SerializeField] int WeaponDamage = 0;
    [SerializeField] float AttackSpeed = 0;
    [SerializeField] float WeaponRange = 0;
    [SerializeField] float Weight = 0;
    [SerializeField] float knockback_force = 0;
    [SerializeField] bool IsRightHanded = true;

    const string weaponName = "Weapon";

    public void Spawn(Transform RightHand, Transform LeftHand, Animator animator)
    {
        DestroyOldWeapon(RightHand,LeftHand);
        if(EquippedPrefab != null)
        {
            Transform HandTransform = GetHandTransform(RightHand, LeftHand);
            GameObject NewWeapon = Instantiate(EquippedPrefab, HandTransform);
            NewWeapon.name = weaponName;
        }
        var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
        if (animatorOverride != null)
        {
            animator.runtimeAnimatorController = animatorOverride;
        }
        else if (overrideController != null)
        {
            animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
        }
    }


    private void DestroyOldWeapon(Transform RightHand, Transform LeftHand)
    {
        Transform oldWeapon = RightHand.Find(weaponName);
        if (oldWeapon == null)
        {
            oldWeapon = LeftHand.Find(weaponName);
        }
        if (oldWeapon == null) return;

        oldWeapon.name = "DESTROYING";
        Destroy(oldWeapon.gameObject);
    }

    private Transform GetHandTransform(Transform righthand, Transform leftHand)
    {
        Transform HandTransform;
        if (IsRightHanded) HandTransform = righthand;
        else HandTransform = leftHand;
        return HandTransform;
    }

    //public bool HasProjectile()
    //{
    //    return WeaponProjectile != null;
    //}

    //public void LaunchProjectile(Transform righthand, Transform leftHand, Transform target)
    //{
    //    Projectile projectileInstance = Instantiate(WeaponProjectile, GetHandTransform(righthand, leftHand).position, Quaternion.identity);
    //    projectileInstance.SetTarget(target);
    //}

    public int GetDamage()
    {
        return WeaponDamage;
    }

    public float GetWeaponRange()
    {
        return WeaponRange;
    }

    public float GetWeaponAttackSpeed()
    {
        return AttackSpeed;
    }

    public float GetWeaponWeight()
    {
        return Weight;
    }

    public float GetKnockBackForce()
    {
        return knockback_force;
    }
}