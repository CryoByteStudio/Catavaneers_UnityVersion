using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SpawnSystem.Standard
{
    [System.Serializable]
    public class EnemyToSpawn
    {
        [Header("Enemy Properties")]
        public EnemyType enemyToSpawnType;

        [Header("Spawn Properties")]
        public int count;
        public SpawnPoint spawnPoint;

        [HideInInspector] public string name;

        /// <summary>
        /// Set name to match the type of enemy that was set to spawn
        /// </summary>
        public void ResetName()
        {
            name = enemyToSpawnType.ToString();
        }
    }

    [System.Serializable]
    public class Wave
    {
        public static int number = 0;
        [HideInInspector] public string name;

        public List<EnemyToSpawn> enemiesToSpawn;
        public float spawnInterval;

        private int enemyCount;
        public int EnemyCount { get { return enemyCount; } }

        /// <summary>
        /// Initialize wave variables
        /// </summary>
        public void Init()
        {
            for (int i = 0; i < enemiesToSpawn.Count; i++)
            {
                if (enemiesToSpawn[i].name != enemiesToSpawn[i].enemyToSpawnType.ToString())
                    enemiesToSpawn[i].ResetName();

                enemyCount += enemiesToSpawn[i].count;
            }
        }

        /// <summary>
        /// Set parameters neccessary to every spawn points in wave settings
        /// </summary>
        public void SetSpawnParams()
        {
            for (int i = 0; i < enemiesToSpawn.Count; i++)
            {
                enemiesToSpawn[i].spawnPoint.SetSpawnParams(enemiesToSpawn, spawnInterval);
            }
        }
    }
}
