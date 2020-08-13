using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class SelectorCompositeTask : CompositeTask
    {
        #region PUBLIC
        public SelectorCompositeTask(string name) : base(name)
        {
        }

        public SelectorCompositeTask(string name, ExecutionContext context) : base(name, context)
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
                    currentChild.Update(context);
                }

                if (currentChild.IsFailure())
                {
                    AdvanceToNextChild();
                }
                // If child succeeded...
                else
                {
                    // ...propagate status up
                    childStatus = currentChild.GetStatus();
                }
            }

            // Handle completed child task
            if (IsSelectorSuccess(childStatus))
            {
                SetStatus(HandleStatusOnChildSuccess(TaskStatus.Success));
            }
            else if (HasReachTheEnd())
            {
                SetStatus(TaskStatus.Failure);
            }

            return GetStatus();
        }

        protected virtual bool IsSelectorSuccess(TaskStatus childStatus)
        {
            return childStatus == TaskStatus.Success;
        }

        /// <summary>
        /// Enabling fine-tune control over how selector task behaves on success
        /// </summary>
        protected virtual TaskStatus HandleStatusOnChildSuccess(TaskStatus childStatus)
        {
            if (childStatus == TaskStatus.Success)
                return childStatus;

            return TaskStatus.Success;
        }
        #endregion
        #region PRIVATE
        #endregion
    }
}