using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrap : MonoBehaviour
{
    [SerializeField] int Damage;
    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.GetComponent<HealthComp>() != null)
        {
            //other.gameObject.GetComponent<HealthComp>().TakeDamage(Damage);
            Destroy(gameObject);
            if (other.gameObject.GetComponent<HealthComp>().myClass == CharacterClass.Player)
            {
                other.gameObject.GetComponent<HealthComp>().TakeDamage(Damage);
                Destroy(gameObject);
            }
            else if (other.gameObject.GetComponent<HealthComp>().myClass == CharacterClass.Enemy)
            {

            }
        }
    }
}
