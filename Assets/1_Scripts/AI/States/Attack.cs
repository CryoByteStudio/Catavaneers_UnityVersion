﻿using System.Collections;
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
        private Weapon equippedWeapon = null;
        private int attackDamage = 0;
        private float attackRange = 0;
        private float attackInterval = 0;
        private float speedOffset = 0;

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
            if (!controller)
                Debug.LogWarning("Controller is not set in Attack state");
            if (!agent)
                agent = controller.Agent;
            if (!animatorController)
                animatorController = controller.AnimatorController;
            if (!equippedWeapon)
                equippedWeapon = controller.EquippedWeapon;
            if (speedOffset == 0)
                speedOffset = Random.Range(-0.2f, 0.2f);

            target = controller.CurrentTarget;
            //attackDamage = controller.AttackDamage;
            attackDamage = controller.BaseAttackDamage + equippedWeapon.GetDamage();
            attackRange = controller.AttackRange;
            //attackInterval = controller.AttackInterval;
            attackInterval = equippedWeapon.GetWeaponAttackSpeed() + speedOffset;
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
                PlayAttackAnimation();
                timeSinceLastAttack = 0;
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

        private void PlayAttackAnimation()
        {
            HealthComp targetHealth = target.GetComponent<HealthComp>();

            if (!targetHealth.IsDead())
            {
                if (animatorController)
                {
                    animatorController.SetTrigger("Attack");
                    animatorController.SetFloat("Chase", 0f);
                }
                //Debug.Log("[" + controller.name + "] dealt " + damage + " to [" + target.name + "]");
            }
            else
            {
                HandleTargetIsDead(targetHealth);
            }
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
                targetHealth.TakeDamage(damage);
                //Debug.Log("[" + controller.name + "] dealt " + damage + " to [" + target.name + "]");
            }
            else
            {
                //HandleTargetIsDead(targetHealth);
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
