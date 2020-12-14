using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public abstract class LeafTask : Task
    {
        #region PUBLIC
        public LeafTask(string name) : base(name)
        {
        }

        public LeafTask(string name, ExecutionContext context) : base(name, context)
        {
        }

        public override bool IsTerminated()
        {
            return false;
        }

        public override string Debug()
        {
            return "Leaf task [" + name + "] - Status [" + Status.ToString() + "]";
        }
        #endregion
        #region PROTECTED
        #endregion
        #region PRIVATE
        #endregion
    }
}