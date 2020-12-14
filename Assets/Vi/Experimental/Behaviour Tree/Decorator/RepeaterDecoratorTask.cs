using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class RepeaterDecoratorTask : DecoratorTask
    {
        #region PUBLIC
        public RepeaterDecoratorTask(Task child) : base("Repeater")
        {
            this.child = child;
        }

        public RepeaterDecoratorTask(Task child, ExecutionContext context) : base("Repeater", context)
        {
            this.child = child;
        }
        #endregion
        #region PROTECTED
        protected override TaskStatus PerformAction()
        {
            if (IsStatusWaiting())
                InitializeTask();
            
            if (child != null)
            {
                SetStatus(child.Update(context));
            }
            else
            {
                SetStatus(TaskStatus.Failure);
            }

            if (child.IsTerminated())
            {
                Reset();
                child.Reset();
            }

            return Status;
        }
        #endregion
        #region PRIVATE
        #endregion
    }
}