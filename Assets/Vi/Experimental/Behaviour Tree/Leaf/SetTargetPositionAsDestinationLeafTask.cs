using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class SetTargetPositionAsDestinationLeafTask : LeafTask
    {
        #region PUBLIC
        public SetTargetPositionAsDestinationLeafTask()
            : base("Set Target Position As Destination")
        {
        }

        public SetTargetPositionAsDestinationLeafTask(ExecutionContext context)
            : base("Set Target Position As Destination", context)
        {
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

            IEntityInfo target = context.Target;
            if (target == null)
            {
                SetStatus(TaskStatus.Failure);
                return Status;
            }

            if (context.SetMoveToPosition(target.Position))
            {
                SetStatus(TaskStatus.Success);
            }
            else
            {
                SetStatus(TaskStatus.Failure);
            }

            return Status;
        }
        #endregion
        #region PRIVATE
        #endregion
    }
}