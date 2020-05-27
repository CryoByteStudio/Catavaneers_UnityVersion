using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamageHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            Debug.Log(other.gameObject.name);
        }
    }
}
