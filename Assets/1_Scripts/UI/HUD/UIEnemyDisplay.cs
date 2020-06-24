using System.Collections;
using UnityEngine;
using TMPro;
using SpawnSystem;
using SpawnSystem.Standard;

public class UIEnemyDisplay : MonoBehaviour
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
            textField.text = string.Format("Enemies Left in This Wave: {0}", SpawnManager.EnemiesAlive);

        if (!spawnManagerReference)
            StartCoroutine(FindSpawnManagerReference());
    }

    private IEnumerator FindSpawnManagerReference()
    {
        spawnManagerReference = FindObjectOfType<SpawnManager>();
        yield return null;
    }
}
