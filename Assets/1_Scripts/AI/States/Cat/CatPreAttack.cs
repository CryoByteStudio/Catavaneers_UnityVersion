using UnityEngine;
using UnityEngine.AI;
using FiniteStateMachine.StatePolymorphism;

namespace AI.States
{
    public class CatPreAttack : State
    {
        private CatAIController controller = null;
        private Animator animatorController = null;
        private Transform target = null;
        private HealthComp targetHealth = null;
        private NavMeshAgent agent = null;
        private Vector3 destination = Vector3.zero;

        public CatPreAttack(CatAIController controller)
        {
            this.controller = controller;
        }

        public override void OnStateEnter()
        {
            Init();

            controller.currentState = AIState.PreAttack;
        }

        public override void Update(float deltaTime)
        {
            PreAttackBehaviour();
        }

        public override void OnStateExit()
        {
            agent.isStopped = true;
        }

        private void Init()
        {
            if (!controller)
                Debug.LogWarning("Controller is not set in PreAttack state");
            if (!agent)
                agent = controller.Agent;
            if (!animatorController)
                animatorController = controller.AnimatorController;

            target = controller.CurrentTarget;
            targetHealth = target.GetComponent<HealthComp>();
        }

        private void Reset()
        {
            destination = Vector3.zero;
            agent.isStopped = false;
        }

        private void PreAttackBehaviour()
        {
            if (!target) return;

            if (animatorController)
                animatorController.SetFloat("Chase", agent.velocity.magnitude);

            if (destination == Vector3.zero)
                destination = GetPosition();

            agent.SetDestination(destination);
        }

        private Vector3 GetPosition()
        {
            Vector3 newPos = Vector3.zero;
            RaycastHit hit;

            if (Physics.Raycast(controller.transform.position, target.position - controller.transform.position, out hit))
            {
                newPos = hit.point;
                newPos.y = controller.transform.position.y;
            }
            else
            {
                newPos = target.position;
            }

            return newPos;
        }
    }
}