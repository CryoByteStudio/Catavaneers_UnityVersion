using System.Collections.Generic;
using UnityEngine;
using ObjectPooling;
using CustomMathLibrary;
//using SpawnSystem.Standard;

namespace SpawnSystem
{
    public class SpawnPoint : MonoBehaviour
    {
        [Header("Spawn Settings")]
        public float spawnRadius = 0;

        private Queue<string> spawnQueue = new Queue<string>();
        private int enemyLeftToSpawn = 0;
        private float spawnInterval = 0;
        private List<string> enemyNames;
        private float timeElapsed = 0;
        private float nextSpawnTime = 0;
        private Vector3 randomPosition;
        private bool canSpawn = true;

        private static ObjectPooler objectPooler;

        // for debugging
        private HealthComp healthComp;

        private void Start()
        {
            if (!objectPooler)
                objectPooler = FindObjectOfType<ObjectPooler>();
        }

        private void Update()
        {
            // if cannot spawn, do nothing
            if (!SpawnManager.CanSpawnEnemy) return;

            // if still has enemy to spawn
            if (enemyLeftToSpawn > 0)
            {
                // ... and is time to spawn
                if (timeElapsed > nextSpawnTime)
                {
                    Spawn(spawnQueue.Dequeue());
                    nextSpawnTime = timeElapsed + spawnInterval;
                }

                if (enemyLeftToSpawn <= 0)
                    spawnQueue.Clear();

                timeElapsed += Time.deltaTime;
            }
        }

        /// <summary>
        /// Spawn a specific enemy from pool at a random position within spawn radius
        /// </summary>
        /// <param name="name"> Name of the enemy to pull from pool </param>
        private void Spawn(string name)
        {
            randomPosition = CustomMathf.RandomPointInCirclePerpendicularToAxis(spawnRadius, Axis.Y) + transform.position;
            healthComp = objectPooler.SpawnFromPool(name, randomPosition, Quaternion.identity).GetComponent<HealthComp>();
            enemyLeftToSpawn--;
            SpawnManager.SetEnemyLeftToSpawn(enemyLeftToSpawn);
            SpawnManager.EnemiesAlive++;

            // for debugging
            GameQuitDebugging();
        }

        /// <summary>
        /// Set health component to debug mode to test game quitting function
        /// </summary>
        private void GameQuitDebugging()
        {
            if (healthComp)
                healthComp.debug = SpawnManager.m_Debug;
        }

        /// <summary>
        /// Set parameters neccessary to spawn enemies
        /// </summary>
        /// <param name="enemiesToSpawn"> List of spawn settings that contains information such as name, count, etc. </param>
        /// <param name="spawnInterval"> The interval which enemy should spawn back to back with </param>
        public void SetSpawnParams(List<Standard.EnemyToSpawn> enemiesToSpawn, float spawnInterval)
        {
            for (int i = 0; i < enemiesToSpawn.Count; i++)
            {
                for (int j = 0; j < enemiesToSpawn[i].count; j++)
                {
                    if (enemiesToSpawn[i].spawnPoint == this)
                        spawnQueue.Enqueue(enemiesToSpawn[i].name);
                }
            }

            enemyLeftToSpawn = spawnQueue.Count;
            this.spawnInterval = spawnInterval;
        }

        /// <summary>
        /// Set parameters neccessary to spawn enemies
        /// </summary>
        /// <param name="enemiesToSpawn"> List of spawn settings that contains information such as name, count, etc. </param>
        /// <param name="spawnInterval"> The interval which enemy should spawn back to back with </param>
        public void SetSpawnParams(List<ScriptableObj.EnemyToSpawn> enemiesToSpawn, float spawnInterval)
        {
            for (int i = 0; i < enemiesToSpawn.Count; i++)
            {
                for (int j = 0; j < enemiesToSpawn[i].count; j++)
                {
                        spawnQueue.Enqueue(enemiesToSpawn[i].name);
                }
            }

            enemyLeftToSpawn = spawnQueue.Count;
            this.spawnInterval = spawnInterval;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, spawnRadius);
        }
    }
}