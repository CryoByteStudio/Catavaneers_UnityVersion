using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WorldTextStartForce : MonoBehaviour
{
    [SerializeField] private float minForce;
    [SerializeField] private float maxForce;
    [SerializeField] private Vector3 forceDirection;

    private new Rigidbody rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        Push();
    }

    public void Push()
    {
        rigidbody.AddForce(forceDirection.normalized * Random.Range(minForce, maxForce));
    }

    public void RandomizeForceDirection()
    {
        forceDirection = Random.onUnitSphere;
    }
}