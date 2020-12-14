using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class SequenceCompositeTask : CompositeTask
    {
        #region PUBLIC
        public SequenceCompositeTask(string name) : base(name)
        {
        }

        public SequenceCompositeTask(string name, ExecutionContext context) : base(name, context)
        {
        }
        #endregion
        #region PROTECTED
        protected override TaskStatus PerformAction()
        {
            if (IsStatusWaiting())
                InitializeTask();

            TaskStatus childStatus = TaskStatus.Running;
            Task currentChild = GetCurrentChild();
            if (currentChild != null)
            {
                if (!currentChild.IsTerminated())
                {
                    childStatus = currentChild.Update(context);
                }
                else if (childStatus != TaskStatus.Running)
                {
                    AdvanceToNextChild();
                }

                // If child failed...
                if (currentChild.IsFailure())
                {
                    // ...propagate status up
                    childStatus = currentChild.Status;
                }
            }

            // Handle completed child task
            if (IsSequenceFailure(childStatus))
            {
                SetStatus(HandleStatusOnChildFailure(childStatus));
            }
            else if (HasReachTheEnd())
            {
                SetStatus(TaskStatus.Success);
            }

            return Status;
        }

        protected virtual bool IsSequenceFailure(TaskStatus childStatus)
        {
            return childStatus == TaskStatus.Failure;
        }

        /// <summary>
        /// Enabling fine-tune control over how sequence task behaves on failure
        /// </summary>
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