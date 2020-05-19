using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeTrap : MonoBehaviour
{
    [SerializeField] float BackToNormal = 0;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //other.GetComponent<PlayerController>().HitByfreezeTrap(true, BackToNormal);
            Debug.Log("Freeze");
            Destroy(gameObject);
        }
    }
}
