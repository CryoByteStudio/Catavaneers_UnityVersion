using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum OpenDirection
{
    Right,
    Left
}

public class AutoDoor : MonoBehaviour
{
    [Header("Automatic Door Settings")]
    public float speed;
    public float stopTime;
    public float moveDistance;
    public OpenDirection eOpenDirection;

    private Vector3 openDirection;
    private Transform doorTransform;
    private bool shouldOpen;
    private bool shouldClose;
    private NavMeshAgent agent;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float snapThreshold = 0.1f;
    private float stopTimeElapsed;

    private void Start()
    {
        doorTransform = transform.GetChild(0);
        startPosition = transform.position;
        openDirection = eOpenDirection == OpenDirection.Right ? Vector3.right : Vector3.left;
        endPosition = startPosition + transform.InverseTransformDirection(openDirection) * moveDistance;
    }

    private void Update()
    {
        if (shouldOpen)
        {
            Open();
        }
        else if (shouldClose)
        {
            stopTimeElapsed += Time.deltaTime;

            if (stopTimeElapsed >= stopTime)
            {
                Close();
            }
        }
    }

    /// <summary>
    /// Move door position to predefined end position and snap to it 
    /// </summary>
    public void Open()
    {
        if (Vector3.Distance(doorTransform.position, endPosition) <= snapThreshold)
        {
            doorTransform.position = endPosition;
            shouldOpen = false;
        }
        else
        {
            doorTransform.Translate((endPosition - startPosition) * speed * Time.deltaTime, Space.World);
        }
    }

    /// <summary>
    /// Move door position to predefined start position and snap to it 
    /// </summary>
    public void Close()
    {
        if (Vector3.Distance(doorTransform.position, startPosition) <= snapThreshold)
        {
            doorTransform.position = startPosition;
            shouldClose = false;
        }
        else
        {
            doorTransform.Translate((startPosition - endPosition) * speed * Time.deltaTime, Space.World);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // override boolean shouldClose when triggered
        shouldOpen = true;
        shouldClose = false;
        stopTimeElapsed = 0;
    }

    private void OnTriggerExit(Collider other)
    {
        shouldClose = true;
    }
}
