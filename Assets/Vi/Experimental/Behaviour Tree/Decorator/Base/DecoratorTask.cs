using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public abstract class DecoratorTask : Task
    {
        #region PUBLIC
        public DecoratorTask(string name) : base(name)
        {
        }

        public DecoratorTask(string name, ExecutionContext context) : base(name, context)
        {
        }

        public override string Debug()
        {
            string logger = "Decorator task [" + name + "] - Status [" + Status.ToString() + "]";

            if (child != null)
            {
                logger += "\n " + child.Debug();
            }

            return logger;
        }
        #endregion
        #region PROTECTED
        protected Task child;
        #endregion
        #region PRIVATE
        #endregion
    }
}