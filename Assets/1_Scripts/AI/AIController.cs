﻿using UnityEngine;
using UnityEngine.AI;
using FiniteStateMachine.StatePolymorphism;
using System.Collections;
using System;

namespace AI
{
    public enum AIState
    {
        FindTarget,
        Chase,
        PreAttack,
        Attack,
        Frenzy,
        Death
    }

    public abstract class AIController : MonoBehaviour
    {
        [Header("Chase Settings")]
        [SerializeField] protected float chaseSpeed = 0;
        [SerializeField] protected float transitionDistanceTolerant = 0;

        [Header("Attack Settings")]
        [SerializeField] protected int attackDamage = 0;
        [SerializeField] protected float attackRange = 0;
        [Range(0f, 1f)]
        [SerializeField] protected float accuracy = 0.8f;

        [Header("Frenzy Settings")]
        [SerializeField] protected float frenzyRadius = 0;
        [SerializeField] protected float frenzySpeed = 0;

        [Header("Death Settings")]
        [SerializeField] protected float deathAnimationDuration = 3;
        [SerializeField] protected float fadeDuration = 5;

        public event Action OnHit;
        public void InvokeOnHit() { if (OnHit != null) OnHit.Invoke(); }

        protected FSM finiteStateMachine = new FSM();
        protected HealthComp healthComponent = null;
        protected float distanceToTarget = Mathf.Infinity;
        protected float distanceToTargetSqr = Mathf.Infinity;
        protected NavMeshAgent navMeshAgentComponent = null;
        [SerializeField] protected Animator animatorControllerComponent = null;
        protected Transform targetPointTransform = null;
        protected float distanceToTargetPointTransform = Mathf.Infinity;
        protected Transform currentTarget = null;
        protected Weapon equippedWeapon;
        protected bool isFrenzy = false;

        public HealthComp HealthComponent { get { return healthComponent; } }
        public float DistanceToTarget { get { return distanceToTarget; } }
        public NavMeshAgent Agent { get { return navMeshAgentComponent; } }
        public Animator AnimatorController { get { return animatorControllerComponent; } }
        public Transform TargetPointTransform { get { return targetPointTransform; } }
        public Transform CurrentTarget { get { return currentTarget; } }
        public float ChaseSpeed { get { return chaseSpeed; } }
        public int BaseAttackDamage { get { return attackDamage; } }
        public float AttackRange { get { return attackRange; } }
        public float Accuracy { get { return accuracy; } }
        public float FrenzyRadius { get { return frenzyRadius; } }
        public float FrenzySpeed { get { return frenzySpeed; } }
        public bool IsFrenzy { get { return isFrenzy; } }
        public float DeathAnimationDuration { get { return deathAnimationDuration; } }
        public float FadeDuration { get { return fadeDuration; } }
        public Weapon EquippedWeapon { get { return equippedWeapon; } }

        protected static HealthComp[] allTargetsWithHealthComponent;
        protected Vector3 startPosition;

        public AIState currentState = AIState.FindTarget;


        //Particle Prefab References:

        public ParticleSystem KickUpGrassL;
        public ParticleSystem KickUpGrassR;



        //gameobject references

        public ParticleSystem hiteffect;
        public ParticleSystem grassR;
        public ParticleSystem grassL;

        public ParticleSystem FreezeEffect;
        public ParticleSystem DamageEffect;
        public ParticleSystem SlowEffect;
        public ParticleSystem ReverseEffect;

        protected bool active;

        protected virtual void OnEnable()
        {
            SetActive(true);
            OverrideNavMeshAgentAutoRepositioning();
            RegisterToEvents();
        }

        protected virtual void OnDisable()
        {
            SetActive(false);
            UnregisterToEvents();
        }

        protected virtual void Start()
        {
            Reset();
            InitializeReferences();
            OverrideNavMeshAgentAutoRepositioning();
            InitializeFSM();

            if (KickUpGrassR)
            {
                grassR = Instantiate(KickUpGrassR, transform.position, Quaternion.identity, null);
            }

            if (KickUpGrassL)
            {
                grassL = Instantiate(KickUpGrassL, transform.position, Quaternion.identity, null);
            }
        }

        protected virtual void Reset()
        {
            allTargetsWithHealthComponent = null;
        }

        protected virtual void Update()
        {
            if (!active) return;

            finiteStateMachine.Update(Time.deltaTime);

            if (currentTarget)
                distanceToTarget = GetProjectedDistanceMagnitude(transform.position, currentTarget.position);

            if (targetPointTransform)
                distanceToTargetPointTransform = GetProjectedDistanceMagnitude(transform.position, targetPointTransform.position);
        }

        protected virtual void RegisterToEvents()
        {
            HealthComp.OnCaravanDestroyed += OnCaravanDestroyedHandler;
        }

        protected virtual void UnregisterToEvents()
        {
            HealthComp.OnCaravanDestroyed -= OnCaravanDestroyedHandler;
        }

        protected virtual void InitializeFSM()
        {
        }

        protected void SetActive(bool value)
        {
            active = value;
        }

        private void OnCaravanDestroyedHandler(HealthComp healthComp)
        {
            
        }

        private void OnHealthChangedHandler(HealthComp healthComp)
        {

        }

        private void InitializeReferences()
        {
            GetAllTargetsWithHealthComponentReference();
            GetAnimatorReference();
            GetNavMeshAgentReference();
            GetHealthComponentReference();
            GetWeaponReference();
        }

        private void GetHealthComponentReference()
        {
            if (!healthComponent)
                healthComponent = GetComponent<HealthComp>();
        }

        private void GetNavMeshAgentReference()
        {
            if (!navMeshAgentComponent)
                navMeshAgentComponent = GetComponent<NavMeshAgent>();
        }

        private void GetAnimatorReference()
        {
            if (!animatorControllerComponent)
                animatorControllerComponent = GetComponent<Animator>();
        }

        private static void GetAllTargetsWithHealthComponentReference()
        {
            if (allTargetsWithHealthComponent == null || allTargetsWithHealthComponent.Length <= 0)
                allTargetsWithHealthComponent = FindObjectsOfType<HealthComp>();
        }

        private void OverrideNavMeshAgentAutoRepositioning()
        {
            if (navMeshAgentComponent)
            {
                startPosition = transform.position;
                navMeshAgentComponent.Warp(startPosition);
            }
        }

        private void GetWeaponReference()
        {
            EnemyWeapon weaponComponent = GetComponent<EnemyWeapon>();
            if (weaponComponent)
            {
                weaponComponent.Init();
                equippedWeapon = weaponComponent.CurrentWeapon;
            }
        }

        public void SetCurrentTarget(Transform target)
        {
            currentTarget = target;
        }

        public void SetTargetPoint(Transform pointTransform)
        {
            targetPointTransform = pointTransform;
        }

        public static float GetProjectedDistanceMagnitude(Vector3 fromPosition, Vector3 targetPosition)
        {
            float distance = Vector3.ProjectOnPlane(targetPosition - fromPosition, Vector3.up).magnitude;
            return distance;
        }

        public static float GetProjectedDistanceSqrMagnitude(Vector3 fromPosition, Vector3 targetPosition)
        {
            float distance = Vector3.ProjectOnPlane(targetPosition - fromPosition, Vector3.up).sqrMagnitude;
            return distance;
        }
        
        public void SetTemporaryMovementSpeed(float speed, float timer)
        {
            StartCoroutine(SetAndRestoreSpeed(speed, timer));
        }
        
        private IEnumerator SetAndRestoreSpeed(float speed, float timer)
        {
            float baseSpeed = 0;

            if (currentState == AIState.Chase)
            {
                baseSpeed = chaseSpeed;
                chaseSpeed = speed;
                navMeshAgentComponent.speed = speed;
                yield return new WaitForSeconds(timer);
                chaseSpeed = baseSpeed;
                navMeshAgentComponent.speed = chaseSpeed;
            }
            else if (currentState == AIState.Frenzy)
            {
                baseSpeed = frenzySpeed;
                frenzySpeed = speed;
                navMeshAgentComponent.speed = speed;
                yield return new WaitForSeconds(timer);
                frenzySpeed = baseSpeed;
                navMeshAgentComponent.speed = frenzySpeed;
            }
            else
            {
                yield return null;
            }
            if(GetComponent<Animator>() != null)
            {
                GetComponent<Animator>().SetFloat("Chase", navMeshAgentComponent.speed);
            }
        }
        
        public void ToggleFrenzyStateWithTimer(float timer)
        {
            StartCoroutine(SetAndRestoreFrenzyState(timer));
        }

        private IEnumerator SetAndRestoreFrenzyState(float timer)
        {
            isFrenzy = !isFrenzy;
            yield return new WaitForSeconds(timer);
            isFrenzy = !isFrenzy;
        }

        //----------------------------------------------------------------------------------------------
        //-------------------------------------- ANIMATION EVENTS --------------------------------------
        //----------------------------------------------------------------------------------------------
        private void Hit()
        {
            //if (hiteffect)
            //{
            //    hiteffect.Play();
            //}

            InvokeOnHit();
        }

        private void FootL()
        {
            if (grassL)
            {
                grassL.Stop();
                grassL.transform.position = new Vector3(transform.position.x, grassL.transform.position.y, transform.position.z);
                grassL.Play();
            }
            else
            {
                Debug.LogWarning("No Particle effect attached to " + name + " for footL");
            }
            //TODO particle FX
        }

        private void FootR()
        {
            if (grassR)
            {
                grassR.Stop();
                grassR.transform.position = new Vector3(transform.position.x, grassR.transform.position.y, transform.position.z);
                grassR.Play();

            }
            else
            {
                Debug.LogWarning("No Particle effect attached to " + name + " for footR");
            }
            //TODO particle FX
        }
        //----------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}