using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCharacterController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private float horizontal, vertical;
    private Vector3 direction;

    private Rigidbody rb;
    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        direction = (horizontal * transform.right + vertical * transform.forward).normalized;
        animator.SetFloat("Walk", direction.magnitude);
        rb.MovePosition(direction * speed * Time.deltaTime + transform.position);

        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.SetTrigger("Attack");
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            animator.SetTrigger("StopAttack");
        }
    }
}
