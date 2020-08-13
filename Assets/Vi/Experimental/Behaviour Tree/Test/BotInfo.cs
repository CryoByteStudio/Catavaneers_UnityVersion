using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace BehaviourTree
{
    public class BotInfo : MonoBehaviour, IEntityInfo
    {
        #region PUBLIC
        public float Health => health;
        public float Speed => speed;
        public Vector3 Position => transform.position;
        public List<IEntityInfo> TargetList => targetList;

        public void SetHealth(float health)
        {
            this.health = health;
        }

        public void SetSpeed(float speed)
        {
            this.speed = speed;
        }
        
        public IEntityInfo GetNemesis()
        {
            KeyValuePair<IEntityInfo, float> enemyDamageData = enemyDamageTracker.FirstOrDefault();
            foreach (var data in enemyDamageTracker)
            {
                if (data.Value > enemyDamageData.Value)
                {
                    enemyDamageData = data;
                }
            }
            return enemyDamageData.Key;
        }

        public IEntityInfo GetClosestTarget()
        {
            IEntityInfo closestEnemy = targetList.FirstOrDefault();
            float closestDistanceToEnemy = Vector3.Distance(Position, closestEnemy.Position);
            foreach (var enemy in targetList)
            {
                float distanceToEnemy = Vector3.Distance(Position, enemy.Position);
                if (distanceToEnemy < closestDistanceToEnemy)
                {
                    closestDistanceToEnemy = distanceToEnemy;
                    closestEnemy = enemy;
                }
            }
            return closestEnemy;
        }
        #endregion
        #region PROTECTED
        #endregion
        #region PRIVATE
        [SerializeField] private float health;
        [SerializeField] private float speed;
        private List<IEntityInfo> targetList;
        private Dictionary<IEntityInfo, float> enemyDamageTracker;

        private void Awake()
        {
            PopulateEnemyTracker();
        }

        private void PopulateEnemyTracker()
        {
            if (enemyDamageTracker == null || enemyDamageTracker.Count <= 0)
            {
                enemyDamageTracker = new Dictionary<IEntityInfo, float>();

                List<IEntityInfo> entities = new List<IEntityInfo>();
                entities.AddRange(FindObjectsOfType<BotInfo>());
                entities.AddRange(FindObjectsOfType<PlayerInfo>());

                entities.Remove(this);

                foreach (var entity in entities)
                {
                    enemyDamageTracker.Add(entity, 0);
                }
            }
        }
        #endregion
    }
}