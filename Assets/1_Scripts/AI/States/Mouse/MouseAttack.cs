using UnityEngine;
using FiniteStateMachine.StatePolymorphism;
using UnityEngine.AI;

namespace AI.States
{
    public class MouseAttack : State
    {
        private MouseAIController controller = null;
        private Animator animatorController = null;
        private NavMeshAgent agent = null;
        private Weapon equippedWeapon = null;
        private int attackDamage = 0;
        private float attackRange = 0;
        private float attackInterval = 0;
        private float accuracy = 0;
        private float speedOffsetThreshold = 0.5f;
        private GameObject damagePopupPrefab = null;
        
        private float timeSinceLastAttack = Mathf.Infinity;
        private Transform target = null;

        public MouseAttack(MouseAIController controller)
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
            attackDamage = controller.BaseAttackDamage + equippedWeapon.GetDamage();
            attackRange = controller.AttackRange;
            attackInterval = equippedWeapon.GetWeaponAttackSpeed();
            accuracy = controller.Accuracy;
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
        
        private bool TimeToAttack()
        {
            return timeSinceLastAttack >= attackInterval;
        }

        private void PlayAttackAnimation()
        {
            animatorController.SetTrigger("Attack");
            animatorController.SetFloat("Chase", 0f);
        }
        
        private void DealDamage()
        {
            HealthComp targetHealth = target.GetComponent<HealthComp>();
            AttackSuccess();
            if (targetHealth && !targetHealth.IsDead())
            {
                if (damagePopupPrefab)
                {
                    DisplayPopup damagePopupInstance = Object.Instantiate(damagePopupPrefab, target.position, Quaternion.identity).GetComponent<DisplayPopup>();

                    bool isTargetDodging = false;

                    if (targetHealth.myClass == CharacterClass.Player && targetHealth.GetComponent<PlayerController>())
                        isTargetDodging = targetHealth.GetComponent<PlayerController>().IsDodging;

                    if (controller.DistanceToTarget <= attackRange && AttackSuccess() && !isTargetDodging)
                    {
                        if (damagePopupInstance)
                            damagePopupInstance.Play(attackDamage);

                        if (controller.hiteffect)
                            controller.hiteffect.Play();

                        // take off health
                        targetHealth.TakeDamage(attackDamage);
                    }
                    else
                    {
                        if (damagePopupInstance)
                        {
                            if (!isTargetDodging)
                                damagePopupInstance.Play("MISS");
                            else
                                damagePopupInstance.Play("DODGE");
                        }
                    }
                }
            }
            else
            {
                HandleTargetIsDead(targetHealth);
            }

            RandomizeAttackInterval();
        }

        private bool AttackSuccess()
        {
            return Random.Range(0, 100f) / 100 < accuracy;
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
            MouseAIController.RemoveFromTargetList(targetHealth);
            controller.SetCurrentTarget(null);
        }
    }
}