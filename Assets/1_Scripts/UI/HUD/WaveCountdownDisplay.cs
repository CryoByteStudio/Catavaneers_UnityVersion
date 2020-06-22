using System.Collections;
using UnityEngine;
using TMPro;
using SpawnSystem;
using SpawnSystem.Standard;

public class WaveCountdownDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text textField;
    [SerializeField] private SpawnManager spawnManagerReference;

    private void Start()
    {
        if (!textField)
            textField = GetComponentInChildren<TMP_Text>();
        if (!spawnManagerReference)
            spawnManagerReference = FindObjectOfType<SpawnManager>();
    }

    private void Update()
    {
        if (spawnManagerReference && textField)
            textField.text = string.Format("Next Wave In: {0:0}s", GetCountdownTime());

        if (!spawnManagerReference)
            StartCoroutine(FindSpawnManagerReference());
    }

    private float GetCountdownTime()
    {
        return spawnManagerReference.NextWaveTime - spawnManagerReference.TimeElapsed;
    }

    private IEnumerator FindSpawnManagerReference()
    {
        spawnManagerReference = FindObjectOfType<SpawnManager>();
        yield return null;
    }
}
