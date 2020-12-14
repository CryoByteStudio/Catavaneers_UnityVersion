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

        public virtual string Name => name;

        public virtual TaskStatus Status => status;

        public virtual ExecutionContext Context => context;

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

        public virtual TaskStatus Update(ExecutionContext context)
        {
            if (context == null)
            {
                SetStatus(TaskStatus.Error);
                UnityEngine.Debug.LogError("Context is null, please provide a valid context.");
                return Status;
            }

            this.context = context;

            if (!IsTerminated())
            {
                SetStatus(PerformAction());
            }
            return Status;
        }

        public virtual bool IsFailure()
        {
            return Status == TaskStatus.Failure;
        }

        public virtual bool IsTerminated()
        {
            return Status == TaskStatus.Success
                || Status == TaskStatus.Failure;
        }

        public virtual bool IsRunning()
        {
            return Status == TaskStatus.Running;
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
            return Status == TaskStatus.Waiting;
        }
        #endregion
        #region PRIVATE
        #endregion
    }
}