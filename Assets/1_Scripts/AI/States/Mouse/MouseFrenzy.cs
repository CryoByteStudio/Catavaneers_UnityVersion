using UnityEngine;
using UnityEngine.AI;
using FiniteStateMachine.StatePolymorphism;
using CustomMathLibrary;

namespace AI.States
{
    public class MouseFrenzy : State
    {
        private MouseAIController controller = null;
        private Animator animatorController = null;
        private NavMeshAgent agent = null;
        private float radius = 0;
        private Vector3 randomPosition;
        private float distanceToRandomPosition = 0;

        public MouseFrenzy(MouseAIController controller)
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
            if (!controller)
                Debug.LogWarning("Controller is not set in Frenzy state");
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
        
        private void GetNewRandomPosition()
        {
            randomPosition = CustomMathf.RandomPointInCirclePerpendicularToAxis(radius, Axis.Y) + controller.transform.position;
        }
    }
}