﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCasting : MonoBehaviour
{
    [SerializeField] float castSizeX = 1f;
    [SerializeField] float castSizeY = 1f;
    [SerializeField] float castSizeZ = 1f;
    float m_MaxDistance;
    float m_Speed;
    bool m_HitDetect;

    CapsuleCollider m_Collider;
    RaycastHit m_Hit;
    Vector3 boxBounds = new Vector3();

    void Start()
    {
        //Choose the distance the Box can reach to
        m_MaxDistance = 300.0f;
        m_Collider = GetComponent<CapsuleCollider>();
        boxBounds = new Vector3(castSizeX, castSizeY, castSizeZ);
    }

    void FixedUpdate()
    {
        //Test to see if there is a hit using a BoxCast
        //Calculate using the center of the GameObject's Collider(could also just use the GameObject's position), half the GameObject's size, the direction, the GameObject's rotation, and the maximum distance as variables.
        //Also fetch the hit data
        m_HitDetect = Physics.BoxCast(transform.position, boxBounds, transform.forward, out m_Hit, transform.rotation, m_MaxDistance);
        if (m_Hit.transform.GetComponent<HealthComp>()!= null)
        {
            //Output the name of the Collider your Box hit
            Debug.Log("Hit : " + m_Hit.collider.name);
        }
    }

    //Draw the BoxCast as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //Check if there has been a hit yet
        if (m_HitDetect)
        {
            //Draw a Ray forward from GameObject toward the hit
            Gizmos.DrawRay(transform.position, transform.forward * m_Hit.distance);
            //Draw a cube that extends to where the hit exists
            Gizmos.DrawWireCube(transform.position + transform.forward * m_Hit.distance, boxBounds);
        }
        //If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(transform.position, transform.forward * m_MaxDistance);
            //Draw a cube at the maximum distance
            Gizmos.DrawWireCube(transform.position + transform.forward * m_MaxDistance, boxBounds);
        }
    }
}