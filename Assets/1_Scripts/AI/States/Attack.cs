using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FiniteStateMachine.StatePolymorphism;

namespace AI.States
{
    public class Attack : State
    {
        // reference from external variables
        private Controller controller = null;
        private Animator animatorController = null;
        private NavMeshAgent agent = null;
        private int attackDamage = 0;
        private float attackRange = 0;
        private float attackInterval = 0;

        // internal variables
        private float timeSinceLastAttack = Mathf.Infinity;
        private Transform target = null;

        public Attack(Controller controller)
        {
            this.controller = controller;
        }

        override public void OnStateEnter()
        {
            Init();

            controller.currentState = AIState.Attack;
        }

        private void Init()
        {
            if (!agent)
                agent = controller.Agent;
            if (!animatorController)
                animatorController = controller.AnimatorController;

            target = controller.CurrentTarget;
            attackDamage = controller.AttackDamage;
            attackRange = controller.AttackRange;
            attackInterval = controller.AttackInterval;
        }

        override public void Update(float deltaTime)
        {
            AttackBehaviour(deltaTime);
        }

        override public void OnStateExit()
        {

        }

        private void LookAtTarget()
        {
            if (!target) return;

            Vector3 lookPoint = target.position;
            lookPoint.y = controller.transform.position.y;
            controller.transform.LookAt(lookPoint);
        }

        /// <summary>
        /// The stuff that will be done in attack mode
        /// </summary>
        private void AttackBehaviour(float deltaTime)
        {
            LookAtTarget();

            if (TimeToAttack())
            {
                timeSinceLastAttack = 0;
                DealDamage(attackDamage);
            }
            
            timeSinceLastAttack += deltaTime;
        }

        /// <summary>
        /// Check if it's time for attack
        /// </summary>
        private bool TimeToAttack()
        {
            return timeSinceLastAttack >= attackInterval;
        }

        /// <summary>
        /// Call TakeDamage method from HealthComp if target is not dead, otherwise handle dead target
        /// </summary>
        /// <param name="damage"> The amout of damage that will be dealt </param>
        private void DealDamage(int damage)
        {
            HealthComp targetHealth = target.GetComponent<HealthComp>();

            if (!targetHealth.IsDead())
            {
                if (animatorController)
                {
                    animatorController.SetTrigger("Attack");
                    animatorController.SetFloat("Chase", 0f);
                }

                targetHealth.TakeDamage(damage);
                //Debug.Log("[" + controller.name + "] dealt " + damage + " to [" + target.name + "]");
            }
            else
            {
                HandleTargetIsDead(targetHealth);
            }
        }

        /// <summary>
        /// Handle the dead target
        /// </summary>
        /// <param name="targetHealth"> The target to be handled </param>
        private void HandleTargetIsDead(HealthComp targetHealth)
        {
            Controller.RemoveFromTargetList(targetHealth);
            controller.SetCurrentTarget(null);
        }
    }
}