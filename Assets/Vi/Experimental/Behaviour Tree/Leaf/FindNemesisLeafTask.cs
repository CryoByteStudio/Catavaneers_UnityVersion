using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class FindNemesisLeafTask : LeafTask
    {
        public FindNemesisLeafTask() : base("Find Nemesis")
        {
        }

        public FindNemesisLeafTask(ExecutionContext context) : base("Find Nemesis", context)
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

            if (context.SetTarget(context.Self.GetNemesis()))
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