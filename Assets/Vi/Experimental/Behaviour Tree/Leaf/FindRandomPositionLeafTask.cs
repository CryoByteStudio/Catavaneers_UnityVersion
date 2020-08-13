using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class FindRandomPositionLeafTask : LeafTask
    {
        #region PUBLIC
        public FindRandomPositionLeafTask()
            : base("Find Random Position")
        {
        }

        public FindRandomPositionLeafTask(float range)
            : base("Find Random Position")
        {
            this.range = range;
        }

        public FindRandomPositionLeafTask(ExecutionContext context)
            : base("Find Random Position", context)
        {
        }

        public FindRandomPositionLeafTask(float range, ExecutionContext context)
            : base("Find Random Position", context)
        {
            this.range = range;
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
                return GetStatus();
            }

            if (context.SetMoveToPosition(GetRandomPosition()))
            {
                SetStatus(TaskStatus.Success);
            }
            else
            {
                SetStatus(TaskStatus.Failure);
            }

            return GetStatus();
        }
        #endregion
        #region PRIVATE
        private float range;

        private Vector3 GetRandomPosition()
        {
            // Find random position logic
            return default;
        }
        #endregion
    }
}