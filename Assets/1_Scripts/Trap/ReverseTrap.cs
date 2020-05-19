using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseTrap : MonoBehaviour
{
    [SerializeField] float BactToNormal = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //other.GetComponent<PlayerController>().HitByReverseTrap(true, BactToNormal);
            Debug.Log("ReverseControl");
            Destroy(gameObject);
        }
    }
}
