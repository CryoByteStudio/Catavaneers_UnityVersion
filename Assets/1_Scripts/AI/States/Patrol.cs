using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FiniteStateMachine.StatePolymorphism;
using AI.PatrolPath;
using UnityEngine.AI;

namespace AI.States
{
    public class Patrol : State
    {
        // reference from external variables
        private Controller controller = null;
        private NavMeshAgent agent = null;
        private PatrolPathGraph patrolPath = null;
        private float speed = 0;
        private float waypointDwellTime = 0;
        private float waypointTolerance = 0;

        // internal variables
        private Vector3 guardPosition = Vector3.zero;
        private float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        private int currentWaypointIndex = 0;

        public Patrol(Controller controller)
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