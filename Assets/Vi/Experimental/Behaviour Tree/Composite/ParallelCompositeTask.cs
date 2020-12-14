using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class ParallelCompositeTask : CompositeTask
    {
        #region PUBLIC
        public ParallelCompositeTask(string name) : base(name)
        {
        }

        public ParallelCompositeTask(string name, ExecutionContext context) : base(name, context)
        {
        }
        #endregion
        #region PROTECTED
        protected override TaskStatus PerformAction()
        {
            int success = 0;
            int failure = 0;

            if (IsStatusWaiting())
                InitializeTask();

            TaskStatus childStatus = TaskStatus.Running;
            foreach (Task child in GetChildren())
            {
                if (!child.IsTerminated())
                    childStatus = child.Update(context);

                if (childStatus == TaskStatus.Success)
                    success++;
                else if (childStatus == TaskStatus.Failure)
                    failure++;

                if (failure > 0)
                {
                    childStatus = TaskStatus.Failure;
                    break;
                }
            }

            if (IsParallelTaskFailure(childStatus))
            {
                SetStatus(HandleStatusOnChildFailure(childStatus));
            }
            else if (success >= GetChildren().Count)
            {
                SetStatus(TaskStatus.Success);
            }

            return Status;
        }

        protected virtual bool IsParallelTaskFailure(TaskStatus childStatus)
        {
            return childStatus == TaskStatus.Failure;
        }

        protected virtual TaskStatus HandleStatusOnChildFailure(TaskStatus childStatus)
        {
            if (childStatus == TaskStatus.Failure)
                return childStatus;

            return TaskStatus.Failure;
        }
        #endregion
        #region PRIVATE
        #endregion
    }
}