using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FiniteStateMachine.StatePolymorphism;

namespace AI.States
{
    public class Investigate : State
    {
        // reference from external variables
        private AIController controller = null;

        public Investigate(AIController controller)
        {
            this.controller = controller;
        }

        override public void OnStateEnter()
        {
            if (!controller)
                Debug.LogWarning("Controller is not set in Patrol state");


        }

        override public void Update(float deltaTime)
        {
            throw new System.NotImplementedException();
        }

        override public void OnStateExit()
        {
            throw new System.NotImplementedException();
        }
    }
}