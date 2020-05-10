using UnityEngine;
using UnityEngine.AI;
using FiniteStateMachine.StatePolymorphism;
using CustomMathLibrary;

namespace AI.States
{
    public class Chase : State
    {
        // reference from external variables
        private Controller controller = null;
        private Transform target = null;
        HealthComp targetHealth = null;
        private NavMeshAgent agent = null;
        Vector3 destination = Vector3.zero;

        public Chase(Controller controller)
        {
            this.controller = controller;
        }

        override public void OnStateEnter()
        {
            if (!controller)
            {
                Debug.LogWarning("Controller is not set in Patrol state");
            }

            if (!agent)
            {
                agent = controller.Agent;
            }

            destination = Vector3.zero;
            agent.isStopped = false;
            target = controller.CurrentTarget;
            targetHealth = target.GetComponent<HealthComp>();

            controller.currentState = AIState.Chase;
        }

        override public void Update(float deltaTime)
        {
            ChaseBehaviour();
        }

        override public void OnStateExit()
        {
            agent.isStopped = true;
        }

        private Vector3 FindPositionNearTarget(Vector3 targetPosition, float radius)
        {
            Vector3 positionNearTarget = CustomMathf.RandomPointInCirclePerpendicularToAxis(radius, CustomMathf.Axis.Y) + targetPosition;
            positionNearTarget.y = targetPosition.y;
            return positionNearTarget;
        }

        /// <summary>
        /// The stuff that will be done in chase mode
        /// </summary>
        private void ChaseBehaviour()
        {
            if (!target) return;

            if (!targetHealth.IsDead())
            {
                if (destination == Vector3.zero)
                {
                    destination = FindPositionNearTarget(target.position, controller.AttackRange);
                    Debug.Log(this + " Destination: " + destination);
                }

                agent.speed = controller.ChaseSpeed;
                agent.SetDestination(destination);
            }
            else
            {
                controller.RemoveFromTargetList(targetHealth);
                controller.SetCurrentTarget(null);
            }
        }
    }
}