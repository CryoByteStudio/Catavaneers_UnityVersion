using FiniteStateMachine.StatePolymorphism;
using UnityEngine;
using UnityEngine.AI;

namespace AI.States
{
    public class CatChase : State
    {
        private CatAIController controller = null;
        private Animator animatorController = null;
        private Transform target = null;
        private HealthComp targetHealth = null;
        private NavMeshAgent agent = null;
        private Vector3 destination = Vector3.zero;

        public CatChase(CatAIController controller)
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

        private void OnEnemyDeathHandler()
        {
            throw new System.NotImplementedException();
        }

        private void Reset()
        {
            destination = Vector3.zero;
            agent.isStopped = false;
        }

        private Vector3 FindPositionNearTarget(Vector3 targetPosition, float radius)
        {
            Vector3 randomPosition = Random.onUnitSphere;
            randomPosition.y = targetPosition.y;
            return (randomPosition - targetPosition).normalized * radius + targetPosition;
        }

        private Vector3 FindOpenPoint(Region targetRegion)
        {
            for (int i = 0; i < targetRegion.InnerRegion.pointsList.Count; i++)
            {
                if (targetRegion.InnerRegion.pointsList[i].IsPointOpen() && !controller.TargetPointTransform)
                {
                    targetRegion.InnerRegion.pointsList[i].SetOccupant(controller.GetComponent<HealthComp>());
                    controller.SetTargetPoint(targetRegion.InnerRegion.pointsList[i].transform);
                    return targetRegion.InnerRegion.pointsList[i].Position;
                }
            }

            for (int i = 0; i < targetRegion.OuterRegion.pointsList.Count; i++)
            {
                if (targetRegion.OuterRegion.pointsList[i].IsPointOpen() && !controller.TargetPointTransform)
                {
                    targetRegion.OuterRegion.pointsList[i].SetOccupant(controller.GetComponent<HealthComp>());
                    controller.SetTargetPoint(targetRegion.OuterRegion.pointsList[i].transform);
                    return targetRegion.OuterRegion.pointsList[i].Position;
                }
            }

            controller.SetTargetPoint(null);
            Vector3 positionNearTarget = FindPositionNearTarget(targetRegion.transform.position, controller.AttackRange - .1f);
            return positionNearTarget;
        }

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
                MouseAIController.RemoveFromTargetList(targetHealth);
                controller.SetCurrentTarget(null);
            }
        }

        public static float GetProjectedDistanceMagnitude(Vector3 fromPosition, Vector3 targetPosition)
        {
            float distance = Vector3.ProjectOnPlane(targetPosition - fromPosition, Vector3.up).magnitude;
            return distance;
        }
    }
}