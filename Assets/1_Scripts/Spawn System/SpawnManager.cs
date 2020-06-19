using System.Collections.Generic;
using UnityEngine;
using ObjectPooling;

using SpawnSystem.Standard;
//using SpawnSystem.ScriptableObj;
using System;
using UnityEngine.SceneManagement;
using Catavaneer.Extensions;

namespace SpawnSystem
{
    public enum SpawnPointOrder
    {
        None = -1,
        One = 0,
        Two = 1,
        Three = 2,
        Four = 3,
        Five = 4,
        Siz = 5,
        Seven = 6,
        Eight = 7,
        Nine = 8,
        Ten = 9
    }

    public enum EnemyType
    {
        None,
        Mouse,
        Cat,
        Dog
    }

    public class SpawnManager : MonoBehaviour
    {
        [Header("Wave Settings")]
        public List<Wave> waves = new List<Wave>();
        public float timeBetweenWaves = 0;


        [Header("Debug Settings")]
        public bool debug = false;

        private float timeElapsed = 0;
        private float nextWaveTime = 0;
        private Wave currentWave;
       

        private static ObjectPooler objectPooler;
        private List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
        
        public static int EnemyLeftToSpawn = 0;
        public static int EnemiesAlive = 0;
        public static bool HasFinishedSpawning = false;
        public static bool CanSpawnEnemy = false;
        public static bool m_Debug = false;

        public float startDelay = 5f;
        public float startTime = 0;
        private void Start()
        {
            Reset();
            
            // update params to start spawning
            startTime = Time.time + startDelay;
        }

        private void Update()
        {
            if (Time.time < startTime) return;
            
            if (HasSpawnedAllEnemies())
            {
                CanSpawnEnemy = false;
            }
            else if (EnemyLeftToSpawn <= 0 && EnemiesAlive <= 0)
            {
                CanSpawnEnemy = true;
            }

            // if still has wave left to spawn
            if (Wave.number < waves.Count && EnemiesAlive <= 0)
            {
                // ... and is time for next wave
                if (timeElapsed > nextWaveTime)
                {
                    SpawnNextWave();
                }

                // otherwise update time elapsed    
                timeElapsed += Time.deltaTime;
            }
        }

        public static void SetEnemyLeftToSpawn(int amount)
        {
            EnemyLeftToSpawn = 0;
            EnemyLeftToSpawn += amount;
        }

        /// <summary>
        /// Check if has spawned all enemies and all waves
        /// </summary>
        private bool HasSpawnedAllEnemies()
        {
            HasFinishedSpawning = EnemyLeftToSpawn <= 0 && Wave.number >= waves.Count;
            return HasFinishedSpawning;
        }

        /// <summary>
        /// Update params for next wave
        /// </summary>
        private void SpawnNextWave()
        {
            Wave.number++;
           // print("Wave Number: " + Wave.number);
            currentWave = waves[Wave.number - 1];
            currentWave.Init();

            // For standard spawn system
            //***************************//
            currentWave.SetSpawnParams();
            //***************************//

            // For scriptable object spawn system
            //***************************//
            //SetSpawnPointParams();
            //***************************//

            nextWaveTime = timeElapsed + currentWave.EnemyCount * currentWave.spawnInterval + timeBetweenWaves;
            //print("Enemy Left To Spawn: " + EnemyLeftToSpawn);
        }

        /// <summary>
        /// Set the spawn parameters for all matching spawn points
        /// </summary>
        //private void SetSpawnPointParams()
        //{
        //    // check all spawn point currently have
        //    for (int i = 0; i < spawnPoints.Count; i++)
        //    {
        //        // check all enemy to spawn in wave object
        //        for (int j = 0; j < currentWave.enemiesToSpawn.Count; j++)
        //        {
        //            // if the order of spawn point in current list (spawnPoints) equal the spawn point order of the enemy to spawn
        //            if ((int)currentWave.enemiesToSpawn[j].spawnPointOrder == i)
        //            {
        //                // set spawn parameter for the appropriate spawn point accordingly
        //                spawnPoints[i].SetSpawnParams(currentWave.enemiesToSpawn, currentWave.spawnInterval);
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// 1. Update name for easy usage...
        /// 2. If params are not correctly assigned, reassigned to base value
        /// </summary>
        [Obsolete]
        private void UpdateWaveVariables()
        {
            for (int i = 0; i < waves.Count; i++)
            {
                // 1
                if (waves[i].name == "")
                    waves[i].name = "Wave " + (i + 1);

                // 2
                if (waves[i].spawnInterval < 0)
                {
                    waves[i].spawnInterval = 0;
                }
                UpdateEnemyToSpawnVariables(i);
            }

            // set static debug variable to match debug value that was set in editor
            m_Debug = debug;
        }

        /// <summary>
        /// 1. Warns designers if their settings is going to cause an error at run time
        /// 2. Set name of enemy to spawn to the correct name base on type it not already matched
        /// </summary>
        /// <param name="index"></param>
        [Obsolete]
        private void UpdateEnemyToSpawnVariables(int index)
        {
            for (int j = 0; j < waves[index].enemiesToSpawn.Count; j++)
            {
                // 1
                if (waves[index].enemiesToSpawn[j].count < 0)
                    Debug.LogError("Enemy count should be a positive number. Error at "
                        + waves[index].enemiesToSpawn[j].name
                        + " in Spawn Manager/Wave Settings/Enemy Properties");

                // 2
                if (waves[index].enemiesToSpawn[j].name != waves[index].enemiesToSpawn[j].enemyToSpawnType.ToString())
                    waves[index].enemiesToSpawn[j].ResetName();
            }
        }

        /// <summary>
        /// Reset all params to base value
        /// </summary>
        private void Reset()
        {
            if (spawnPoints.Count <= 0)
                spawnPoints = transform.GetAllComponentsOfTypeInHierachy<SpawnPoint>();

            Wave.number = 0;
            objectPooler = FindObjectOfType<ObjectPooler>();
            timeElapsed = 0;
            EnemyLeftToSpawn = 0;
            EnemiesAlive = 0;
            HasFinishedSpawning = false;
            CanSpawnEnemy = true;
        }

        private void OnValidate()
        {
            //UpdateWaveVariables();

            // set static debug variable to match debug value that was set in editor
            m_Debug = debug;
        }
    }
}