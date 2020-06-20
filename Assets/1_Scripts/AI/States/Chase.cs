using UnityEngine;
using UnityEngine.AI;
using FiniteStateMachine.StatePolymorphism;
using CustomMathLibrary;

namespace AI.States
{
    public class Chase : State
    {
        private Controller controller = null;
        private Animator animatorController = null;
        private Transform target = null;
        private HealthComp targetHealth = null;
        private NavMeshAgent agent = null;
        private Vector3 destination = Vector3.zero;

        public Chase(Controller controller)
        {
            this.controller = controller;
        }

        override public void OnStateEnter()
        {
            Init();

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

        private void Init()
        {
            if (!controller)
                Debug.LogWarning("Controller is not set in Chase state");
            if (!agent)
                agent = controller.Agent;
            if (!animatorController)
                animatorController = controller.AnimatorController;

            target = controller.CurrentTarget;
            targetHealth = target.GetComponent<HealthComp>();

            Reset();
        }

        private void Reset()
        {
            destination = Vector3.zero;
            agent.isStopped = false;
        }

        /// <summary>
        /// Get random position inside circle with radius relative to a specified target position.
        /// </summary>
        /// <param name="targetPosition"> The target position that the random position will be picked base on </param>
        /// <param name="radius"> The radius in which the random point will be picked from </param>
        private Vector3 FindPositionNearTarget(Vector3 targetPosition, float radius)
        {
            Vector3 positionNearTarget = CustomMathf.RandomPointInCirclePerpendicularToAxis(radius, Axis.Y) + targetPosition;
            positionNearTarget.y = targetPosition.y;
            return positionNearTarget;
        }

        /// <summary>
        /// Find the position of the open point inside the target region.
        /// Return random position inside circle with attack radius if no open point found.
        /// Set target point transform for transition condition in controller.
        /// </summary>
        /// <param name="targetRegion"> The target region to get open point from </param>
        private Vector3 FindOpenPoint(Region targetRegion)
        {
            for (int i = 0; i < targetRegion.InnerRegion.pointsList.Count; i++)
            {
                if (targetRegion.InnerRegion.pointsList[i].IsPointOpen())
                {
                    targetRegion.InnerRegion.pointsList[i].SetOccupant(controller.GetComponent<HealthComp>());
                    controller.SetTargetPoint(targetRegion.InnerRegion.pointsList[i].transform);
                    return targetRegion.InnerRegion.pointsList[i].Position;
                }
            }

            for (int i = 0; i < targetRegion.OuterRegion.pointsList.Count; i++)
            {
                if (targetRegion.OuterRegion.pointsList[i].IsPointOpen())
                {
                    targetRegion.OuterRegion.pointsList[i].SetOccupant(controller.GetComponent<HealthComp>());
                    controller.SetTargetPoint(targetRegion.OuterRegion.pointsList[i].transform);
                    return targetRegion.OuterRegion.pointsList[i].Position;
                }
            }

            controller.SetTargetPoint(null);
            return FindPositionNearTarget(targetRegion.transform.position, controller.AttackRange);
        }

        /// <summary>
        /// The stuff that will be done in chase mode
        /// </summary>
        private void ChaseBehaviour()
        {
            if (!target) return;

            if (!targetHealth.IsDead())
            {
                if (animatorController)
                    animatorController.SetFloat("Chase", agent.velocity.magnitude);

                if (targetHealth.myClass == CharacterClass.Caravan)
                {
                    if (destination == Vector3.zero)
                    {
                        destination = FindOpenPoint(target.GetComponent<Region>());
                    }
                }
                else
                {
                    destination = target.position;
                }

                agent.speed = controller.ChaseSpeed;
                agent.SetDestination(destination);
            }
            else
            {
                Controller.RemoveFromTargetList(targetHealth);
                controller.SetCurrentTarget(null);
            }
        }
    }
}