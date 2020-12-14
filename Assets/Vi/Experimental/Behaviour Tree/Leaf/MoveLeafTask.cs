using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class MoveLeafTask : LeafTask
    {
        #region PUBLIC
        public MoveLeafTask() : base("Move")
        {
        }

        public MoveLeafTask(float distanceTolerantThreshold) : base("Move to Target")
        {
            if (distanceTolerantThreshold > minimumDistanceTolerantThreshold)
                this.distanceTolerantThreshold = distanceTolerantThreshold;
        }

        public MoveLeafTask(ExecutionContext context) : base("Move", context)
        {
        }

        public MoveLeafTask(float distanceTolerantThreshold, ExecutionContext context) : base("Move to Target", context)
        {
            if (distanceTolerantThreshold > minimumDistanceTolerantThreshold)
                this.distanceTolerantThreshold = distanceTolerantThreshold;
        }

        public override bool IsFailure()
        {
            return context.Target == null;
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

            Vector3 currentPosition = context.Self.Position;
            float distanceToDestination = Vector3.Distance(currentPosition, context.Destination);
            if (distanceToDestination <= distanceTolerantThreshold)
            {
                SetStatus(TaskStatus.Success);
            }
            else
            {
                SetStatus(Move());
            }

            return Status;
        }
        #endregion
        #region PRIVATE
        private float minimumDistanceTolerantThreshold = 1.0f;
        private float distanceTolerantThreshold = 1.0f;

        private TaskStatus Move()
        {
            if (!context.HasObjective)
                return TaskStatus.Failure;

            Vector3 moveDirection = (context.Destination - context.Self.transform.position).normalized;
            context.Self.transform.Translate(context.Self.Speed * moveDirection * Time.fixedDeltaTime);

            return TaskStatus.Running;
        }
        #endregion
    }
}