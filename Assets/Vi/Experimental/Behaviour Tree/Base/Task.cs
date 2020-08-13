using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public abstract class Task
    {
        #region PUBLIC
        public enum TaskStatus
        {
            Waiting,
            Running,
            Failure,
            Success,
            Error
        }

        public string Name { get => name; }

        public Task(string name)
        {
            this.name = name;
            status = TaskStatus.Waiting;
        }

        public Task(string name, ExecutionContext context)
        {
            this.name = name;
            this.context = context;
            status = TaskStatus.Waiting;
        }

        public virtual TaskStatus Update()
        {
            return Update(context);
        }

        public virtual TaskStatus Update(ExecutionContext context)
        {
            if (context == null)
            {
                SetStatus(TaskStatus.Error);
                UnityEngine.Debug.LogError("Context is [" + context + "], please provide a valid context.");
                return GetStatus();
            }

            this.context = context;

            if (!IsTerminated())
            {
                SetStatus(PerformAction());
            }
            return GetStatus();
        }

        public virtual bool IsFailure()
        {
            return GetStatus() == TaskStatus.Failure;
        }

        public virtual bool IsTerminated()
        {
            return GetStatus() == TaskStatus.Success
                || GetStatus() == TaskStatus.Failure;
        }

        public virtual TaskStatus GetStatus()
        {
            return status;
        }

        public virtual void Reset()
        {
            SetStatus(TaskStatus.Waiting);
        }

        public abstract string Debug();
        #endregion
        #region PROTECTED
        protected string name;
        protected TaskStatus status;
        protected ExecutionContext context;

        protected abstract TaskStatus PerformAction();

        protected virtual void SetStatus(TaskStatus status)
        {
            this.status = status;
        }

        protected virtual void InitializeTask()
        {
            SetStatus(TaskStatus.Running);
        }

        protected virtual bool IsStatusWaiting()
        {
            return GetStatus() == TaskStatus.Waiting;
        }
        #endregion
        #region PRIVATE
        #endregion
    }
}