using System;
using UnityEngine;
using DG.Tweening;
using CustomMathLibrary;
using Random = UnityEngine.Random;
using System.Collections;

[RequireComponent(typeof(Light))]
public class CampFirePointLight : MonoBehaviour
{
    [SerializeField] private bool flicker;
    [SerializeField] private Transform centerTransform;
    [SerializeField] private float centerRadius;
    [SerializeField] private float distanceTolerance;
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float minIntensity;
    [SerializeField] private float maxIntensity;
    [SerializeField] private float intensityTolerance;
    [SerializeField] private float minIntensityChangeSpeed;
    [SerializeField] private float maxIntensityChangeSpeed;

    private float moveSpeed;
    private float intensityChangeSpeed;
    private Vector3 randomPosition;
    private Vector3 direction;
    private float distanceToTarget;
    private float randomIntensity;
    private float randomIntensityDuration;

    private Light pointLight;

    private void Start()
    {
        pointLight = GetComponent<Light>();

        StartCoroutine(MoveLight());
        StartCoroutine(FlickerLight());
    }

    private IEnumerator FlickerLight()
    {
        while (flicker)
        {
            randomIntensity = Random.Range(minIntensity, maxIntensity);
            intensityChangeSpeed = Random.Range(minIntensityChangeSpeed, maxIntensityChangeSpeed);

            while (Mathf.Abs(pointLight.intensity - randomIntensity) > intensityTolerance)
            {
                if (pointLight.intensity < randomIntensity)
                    pointLight.intensity += Time.deltaTime * intensityChangeSpeed;
                else
                    pointLight.intensity -= Time.deltaTime * intensityChangeSpeed;
                yield return null;
            }
            yield return null;
        }
    }

    private IEnumerator MoveLight()
    {
        while (flicker)
        {
            randomPosition = CustomMathf.GetRandomPointInSphere(centerTransform.position, centerRadius);
            distanceToTarget = Vector3.Distance(transform.position, randomPosition);
            moveSpeed = Random.Range(minSpeed, maxSpeed);

            while (distanceToTarget > distanceTolerance)
            {
                direction = (randomPosition - transform.position).normalized;
                distanceToTarget = Vector3.Distance(transform.position, randomPosition);
                transform.Translate(direction * moveSpeed * Time.deltaTime);
                yield return null;
            }
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(centerTransform.position, centerRadius);
    }
}