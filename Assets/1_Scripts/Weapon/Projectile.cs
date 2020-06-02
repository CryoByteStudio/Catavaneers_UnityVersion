using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Transform target;
    [SerializeField] float ProjectileSpeed = 1;
    [SerializeField] int WeaponDamage = 0;

    // Update is called once per frame
    private void Start()
    {

    }
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * ProjectileSpeed);
    }

    public void SetTarget(Transform target)
    {
        transform.LookAt(target.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<HealthComp>().TakeDamage(WeaponDamage);
            Destroy(gameObject);
        }
    }
}
