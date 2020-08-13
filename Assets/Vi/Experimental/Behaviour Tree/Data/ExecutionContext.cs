using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class ExecutionContext
    {
        #region PUBLIC
        public BotInfo Self => self;
        public IEntityInfo Target => target;
        public Vector3 Destination => destination;
        public bool HasObjective => hasObjective;

        public ExecutionContext()
        {
            hasObjective = false;
        }

        public bool SetSelf(BotInfo self)
        {
            this.self = self;
            return self != null;
        }

        public bool SetTarget(IEntityInfo target)
        {
            this.target = target;
            return target != null;
        }

        public bool SetMoveToPosition(Vector3 destination)
        {
            hasObjective = true;
            this.destination = destination;
            return !IsPositionOutOfBound(destination);
        }

        public bool IsPositionOutOfBound(Vector3 position)
        {
            // check if position out of bound logic
            return false;
        }

        public void Reset()
        {
            hasObjective = false;
        }
        #endregion
        #region PROTECTED
        #endregion
        #region PRIVATE
        private BotInfo self;
        private IEntityInfo target;
        private Vector3 destination;
        private bool hasObjective;
        #endregion
    }
}