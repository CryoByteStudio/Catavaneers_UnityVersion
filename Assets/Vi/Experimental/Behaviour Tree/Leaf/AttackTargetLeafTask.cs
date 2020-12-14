using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class AttackTargetLeafTask : LeafTask
    {
        #region PUBLIC
        public AttackTargetLeafTask() : base("Attack Target")
        {
            lastAttackTime = Mathf.Infinity;
        }

        public AttackTargetLeafTask(float attackPerSec) : base("Attack Target")
        {
            lastAttackTime = Mathf.Infinity;
            attackInterval = ConvertAttackPerSecToInterval(attackPerSec);
        }

        public AttackTargetLeafTask(ExecutionContext context) : base("Attack Target", context)
        {
            lastAttackTime = Mathf.NegativeInfinity;
        }

        public AttackTargetLeafTask(float attackPerSec, ExecutionContext context) : base("Attack Target", context)
        {
            lastAttackTime = Mathf.NegativeInfinity;
            attackInterval = ConvertAttackPerSecToInterval(attackPerSec);
        }
        #endregion
        #region PROTECTED
        protected override TaskStatus PerformAction()
        {
            if (IsStatusWaiting())
                InitializeTask();

            if (context == null)
            {
                SetStatus(TaskStatus.Failure);
                return Status;
            }

            // attack!!!

            if (Time.time > lastAttackTime)
            {
                UnityEngine.Debug.Log("Attacked!!!");
                lastAttackTime = Time.time + attackInterval;
                SetStatus(TaskStatus.Success);
            }
            else
            {
                UnityEngine.Debug.Log("Attack on cooldown");
                SetStatus(TaskStatus.Failure);
            }

            return Status;
        }
        #endregion
        #region PRIVATE
        private float lastAttackTime;
        private float attackInterval = 1f;

        private float ConvertAttackPerSecToInterval(float attackPerSec)
        {
            return 1f / attackPerSec;
        }
        #endregion
    }
}