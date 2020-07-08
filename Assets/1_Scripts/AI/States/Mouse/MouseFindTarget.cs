﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FiniteStateMachine.StatePolymorphism;

namespace AI.States
{
    public class MouseFindTarget : State
    {
        private MouseAIController controller = null;
        private Animator animatorController = null;
        private List<HealthComp> targets = new List<HealthComp>();
        private Transform target = null;
        private NavMeshPath path = new NavMeshPath();

        public MouseFindTarget(MouseAIController controller)
        {
            this.controller = controller;
        }

        override public void OnStateEnter()
        {
            Init();

            controller.currentState = AIState.FindTarget;
        }

        private void Init()
        {
            if (!controller)
                Debug.LogWarning("Controller is not set in FindTarget state");
            if (!animatorController)
                animatorController = controller.AnimatorController;
        }

        override public void Update(float deltaTime)
        {
            if (target) return;

            FindTargetBehaviour();
        }

        override public void OnStateExit()
        {

        }

        private void FindTargetBehaviour()
        {
            if (animatorController)
                animatorController.SetFloat("Chase", 0f);

            FindAndSetTarget();
        }

        private void FindAndSetTarget()
        {
            targets = MouseAIController.Targets;
            target = FindClosestTarget();
            controller.SetCurrentTarget(target);
        }

        private Transform FindClosestTarget()
        {
            Transform closestTarget = null;
            float closestDistance = Mathf.Infinity;

            if (targets.Count > 0)
            {
                closestTarget = targets[0].transform;
                closestDistance = GetPathLength(closestTarget.position);

                for (int i = 1; i < targets.Count; i++)
                {
                    float distanceToTarget = GetPathLength(targets[i].transform.position);

                    if (distanceToTarget < closestDistance)
                    {
                        closestTarget = targets[i].transform;
                        closestDistance = distanceToTarget;
                    }
                }
            }

            return closestTarget;
        }
        
        private float GetPathLength(Vector3 target)
        {
            float length = 0;

            if (NavMesh.CalculatePath(controller.transform.position, target, NavMesh.AllAreas, path))
            {
                length += Vector3.Distance(controller.transform.position, path.corners[0]);

                for (int i = 0; i < path.corners.Length - 1; i++)
                {
                    length += Vector3.Distance(path.corners[i], path.corners[i + 1]);
                }

                length += Vector3.Distance(path.corners[path.corners.Length - 1], target);
            }
            else
            {
                length = Vector3.Distance(controller.transform.position, target);
            }

            return length;
        }
    }
}