using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class WaitLeafTask : LeafTask
    {
        #region PUBLIC
        public WaitLeafTask(float waitTime) : base("Wait")
        {
            timeElapsed = 0;
        }

        public WaitLeafTask(float waitTime, ExecutionContext context) : base("Wait", context)
        {
            timeElapsed = 0;
        }

        public override void Reset()
        {
            base.Reset();
            timeElapsed = 0;
        }
        #endregion
        #region PROTECTED
        protected override TaskStatus PerformAction()
        {
            if (IsStatusWaiting())
                InitializeTask();

            if (timeElapsed >= waitTime)
            {
                SetStatus(TaskStatus.Success);
            }
            else
            {
                timeElapsed += Time.fixedDeltaTime;
            }

            return Status;
        }
        #endregion
        #region PRIVATE
        private float waitTime;
        private float timeElapsed;
        #endregion
    }
}