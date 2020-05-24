using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPackPickUp : MonoBehaviour
{
    enum HealthPackType { Small = 10, Medium = 25, Large = 50 }

    [SerializeField] HealthPackType HealthPack;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            HealthComp Health = other.gameObject.GetComponent<HealthComp>();
            if (Health != null)
                Health.AddHealth((int)HealthPack);
            Destroy(gameObject);
        }
    }
}
