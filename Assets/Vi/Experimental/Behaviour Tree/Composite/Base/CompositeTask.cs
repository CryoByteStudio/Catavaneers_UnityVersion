using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public abstract class CompositeTask : Task
    {
        #region PUBLIC
        public CompositeTask(string name) : base(name)
        {
        }

        public CompositeTask(string name, ExecutionContext context) : base(name, context)
        {
        }

        public virtual List<Task> GetChildren()
        {
            return children;
        }

        public virtual Task GetCurrentChild()
        {
            Task currentChild = null;

            if (HasCurrentChild())
                currentChild = children[currentChildID];

            return currentChild;
        }

        public virtual bool AddChild(Task child)
        {
            if (!children.Contains(child))
            {
                children.Add(child);
                return true;
            }

            return false;
        }

        public override void Reset()
        {
            base.Reset();

            foreach (Task child in GetChildren())
            {
                child.Reset();
            }
        }

        public override string Debug()
        {
            string logger = "Composite task [" + name + "] - Status [" + GetStatus().ToString() + "]";

            if (GetCurrentChild() != null)
            {
                logger += "\n " + GetCurrentChild().Debug();
            }

            return logger;
        }
        #endregion
        #region PROTECTED
        protected List<Task> children = new List<Task>();
        protected int currentChildID;

        protected virtual bool AdvanceToNextChild()
        {
            currentChildID++;
            return HasCurrentChild();
        }

        protected bool HasCurrentChild()
        {
            return currentChildID < children.Count && children[currentChildID] != null;
        }

        protected bool HasReachTheEnd()
        {
            return currentChildID >= GetChildren().Count;
        }

        protected override void InitializeTask()
        {
            base.InitializeTask();
            currentChildID = 0;
        }
        #endregion
        #region PRIVATE
        #endregion
    }
}