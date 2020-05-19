using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FiniteStateMachine.StatePolymorphism;

namespace AI.States
{
    public class FindTarget : State
    {
        // reference from external variables
        private Controller controller = null;
        //private NavMeshAgent agent = null;
        private List<HealthComp> targets = new List<HealthComp>();
        private Transform target = null;
        private float distanceToTarget = Mathf.Infinity;
        private NavMeshPath path = new NavMeshPath();

        public FindTarget(Controller controller)
        {
            this.controller = controller;
        }

        override public void OnStateEnter()
        {
            distanceToTarget = controller.DistanceToTarget;
            targets = controller.GetTargets();
            target = FindClosestTarget();
            controller.SetCurrentTarget(target);
            controller.currentState = AIState.FindTarget;
        }

        override public void Update(float deltaTime)
        {
            if (target) return;
            FindTargetBehaviour();
        }

        override public void OnStateExit()
        {

        }

        /// <summary>
        /// The stuff that will be done in find target mode
        /// </summary>
        private void FindTargetBehaviour()
        {
            // find target
            target = FindClosestTarget();

            // set target
            controller.SetCurrentTarget(target);
        }

        /// <summary>
        /// Find the closest target to the agent
        /// </summary>
        /// <returns></returns>
        private Transform FindClosestTarget()
        {
            Transform closestTarget = null;
            float closestDistance = Mathf.Infinity;

            if (targets.Count > 0)
            {
                closestTarget = targets[0].transform;
                //closestDistance = Vector3.Distance(controller.transform.position, closestTarget.position);
                closestDistance = GetPathLength(closestTarget.position);

                for (int i = 1; i < targets.Count; i++)
                {
                    //float distanceToTarget = Vector3.Distance(controller.transform.position, targets[i].transform.position);
                    float distanceToTarget = GetPathLength(targets[i].transform.position);

                    if (distanceToTarget < closestDistance)
                    {
                        closestTarget = targets[i].transform;
                        closestDistance = distanceToTarget;
                    }
                }
            }
            else
            {
                Debug.Log("No more target");
            }

            return closestTarget;
        }

        /// <summary>
        /// Get the path length on navigation mesh if available, otherwise return a direct distance in 3D space
        /// </summary>
        /// <param name="target"> The location of the target </param>
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