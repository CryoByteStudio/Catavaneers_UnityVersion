using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTrap : MonoBehaviour
{
    [SerializeField] float BackToNormal = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //other.GetComponent<PlayerController>().HitBySlowTrap(true, BackToNormal);
            Debug.Log("Slowed");
            Destroy(gameObject);
        }
    }
}
