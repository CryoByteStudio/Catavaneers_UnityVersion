using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script write by William

public class Shot : MonoBehaviour
{

    public Arrow arrowPrefab;
    public Transform arrowSpawnPoint;
    public float arrowForce = 20f;

    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Arrow arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, transform.rotation);
            arrow.target = target;
            Rigidbody rb = arrow.GetComponent<Rigidbody>();
            //rb.AddForce(arrowSpawnPoint.transform.forward * 500);
        }
    }
}
