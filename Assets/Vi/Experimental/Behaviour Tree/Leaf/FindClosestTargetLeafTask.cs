using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class FindClosestTargetLeafTask : LeafTask
    {
        public FindClosestTargetLeafTask() : base("Find Closest Target")
        {
        }

        public FindClosestTargetLeafTask(ExecutionContext context) : base("Find Closest Target", context)
        {
        }

        protected override TaskStatus PerformAction()
        {
            if (IsStatusWaiting())
                InitializeTask();

            if (context == null)
            {
                SetStatus(TaskStatus.Failure);
                return GetStatus();
            }

            if (context.SetTarget(context.Self.GetClosestTarget()))
            {
                SetStatus(TaskStatus.Success);
            }
            else
            {
                SetStatus(TaskStatus.Failure);
            }

            return GetStatus();
        }
    }
}