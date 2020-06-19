using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FiniteStateMachine.StatePolymorphism;
using CustomMathLibrary;

namespace AI.States
{
    public class Frenzy : State
    {
        // reference from external variables
        private Controller controller = null;
        private Animator animatorController = null;
        private NavMeshAgent agent = null;
        private float radius = 0;
        private Vector3 randomPosition;
        private float distanceToRandomPosition = 0;
        
        public Frenzy(Controller controller)
        {
            this.controller = controller;
        }

        override public void OnStateEnter()
        {
            Init();

            controller.currentState = AIState.Frenzy;
        }

        private void Init()
        {
            if (!agent)
                agent = controller.Agent;
            if (!animatorController)
                animatorController = controller.AnimatorController;

            agent.isStopped = false;
            radius = controller.FrenzyRadius;
            GetNewRandomPosition();
        }

        override public void Update(float deltaTime)
        {
            FrenzyBehaviour();
        }

        override public void OnStateExit()
        {
            agent.isStopped = true;
        }

        /// <summary>
        /// The stuff that will be done in frenzy mode
        /// </summary>
        private void FrenzyBehaviour()
        {
            if (animatorController)
                animatorController.SetFloat("Chase", agent.velocity.magnitude);

            distanceToRandomPosition = Vector3.Distance(controller.transform.position, randomPosition);

            if (distanceToRandomPosition <= 0.2f)
            {
                GetNewRandomPosition();
            }

            agent.speed = controller.FrenzySpeed;
            agent.SetDestination(randomPosition);
        }

        /// <summary>
        /// Get a new random position
        /// </summary>
        private void GetNewRandomPosition()
        {
            randomPosition = CustomMathf.RandomPointInCirclePerpendicularToAxis(radius, Axis.Y) + controller.transform.position;
        }
    }
}