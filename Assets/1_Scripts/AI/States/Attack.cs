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
        private Weapon equippedWeapon = null;
        private int attackDamage = 0;
        private float attackRange = 0;
        private float attackInterval = 0;
        private float speedOffsetThreshold = 0.5f;
        private GameObject damagePopupPrefab = null;

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
            controller.OnHit += DealDamage;
        }

        private void Init()
        {
            if (!damagePopupPrefab)
                damagePopupPrefab = Resources.Load("Damage Popup") as GameObject;
            if (!controller)
                Debug.LogWarning("Controller is not set in Attack state");
            if (!agent)
                agent = controller.Agent;
            if (!animatorController)
                animatorController = controller.AnimatorController;
            if (!equippedWeapon)
                equippedWeapon = controller.EquippedWeapon;

            target = controller.CurrentTarget;
            //attackDamage = controller.AttackDamage;
            attackDamage = controller.BaseAttackDamage + equippedWeapon.GetDamage();
            attackRange = controller.AttackRange;
            //attackInterval = controller.AttackInterval;
            attackInterval = equippedWeapon.GetWeaponAttackSpeed();
        }

        override public void Update(float deltaTime)
        {
            AttackBehaviour(deltaTime);
        }

        override public void OnStateExit()
        {
            controller.OnHit -= DealDamage;
        }

        private void LookAtTarget()
        {
            Vector3 lookPoint = target.position;
            lookPoint.y = controller.transform.position.y;
            controller.transform.LookAt(lookPoint);
        }

        /// <summary>
        /// The stuff that will be done in attack mode
        /// </summary>
        private void AttackBehaviour(float deltaTime)
        {
            if (!target) return;

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
            animatorController.SetTrigger("Attack");
            animatorController.SetFloat("Chase", 0f);
        }

        /// <summary>
        /// Call TakeDamage method from HealthComp if target is not dead, otherwise handle dead target
        /// </summary>
        /// <param name="damage"> The amout of damage that will be dealt </param>
        private void DealDamage()
        {
            HealthComp targetHealth = target.GetComponent<HealthComp>();

            if (targetHealth && !targetHealth.IsDead())
            {
                if (damagePopupPrefab)
                {
                    DamagePopup damagePopupInstance = Object.Instantiate(damagePopupPrefab, target.position, Quaternion.identity).GetComponent<DamagePopup>();

                    if (controller.DistanceToTarget <= attackRange)
                    {
                        if (damagePopupInstance)
                            damagePopupInstance.Play(attackDamage);

                        if (controller.hiteffect)
                            controller.hiteffect.Play();

                        targetHealth.TakeDamage(attackDamage);
                    }
                    else
                    {
                        if (damagePopupInstance)
                            damagePopupInstance.Play("MISS");
                    }
                }
            }
            else
            {
                HandleTargetIsDead(targetHealth);
            }

            RandomizeAttackInterval();
        }

        private void RandomizeAttackInterval()
        {
            attackInterval = Random.Range(-speedOffsetThreshold / 2, speedOffsetThreshold / 2);
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
