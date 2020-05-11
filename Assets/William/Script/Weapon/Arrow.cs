using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    Rigidbody Arrowrb;

    //private float lifeTimer = 2f;
    //private float timer;
    //private bool hitSomething = false;

    [SerializeField] float ProjectileSpeed = 10;
    [SerializeField] float Damage;

    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        Arrowrb = GetComponent<Rigidbody>();
        transform.LookAt(target.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Arrowrb.MoveRotation(Quaternion.LookRotation(Arrowrb.velocity * -1f, transform.up));
        transform.Translate(Vector3.forward * Time.deltaTime * ProjectileSpeed);
    }
}
