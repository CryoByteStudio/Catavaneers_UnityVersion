using SpawnSystem;
using SpawnSystem.Standard;
using System.Collections;
using TMPro;
using UnityEngine;

public class WaveDisplay : MonoBehaviour
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
            textField.text = string.Format("WAVE {0}/{1}", Wave.number, spawnManagerReference.TotalWaves);

        if (!spawnManagerReference)
            StartCoroutine(FindSpawnManagerReference());
    }

    private IEnumerator FindSpawnManagerReference()
    {
        spawnManagerReference = FindObjectOfType<SpawnManager>();
        yield return null;
    }
}
