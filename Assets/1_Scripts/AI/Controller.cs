using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FiniteStateMachine.StatePolymorphism;
using AI.States;
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

    public enum EnemyType
    {
        Mouse,
        Cat,
        Dog
    }

    public class Controller : MonoBehaviour
    {
        [Header("Core Settings")]
        [SerializeField] private EnemyType type = EnemyType.Cat;

        [Header("Chase Settings")]
        [SerializeField] private float chaseSpeed = 0;
        [SerializeField] private float transitionDistanceTolerant = 0;

        [Header("Attack Settings")]
        [SerializeField] private int attackDamage = 0;
        [SerializeField] private float attackRange = 0;
        [SerializeField] private float attackInterval = 0;
        [Range(0f, 1f)]
        [SerializeField] private float accuracy = 0.8f;

        [Header("Frenzy Settings")]
        [SerializeField] private float frenzyRadius = 0;
        [SerializeField] private float frenzySpeed = 0;

        [Header("Death Settings")]
        [SerializeField] private float deathAnimationDuration = 3;
        [SerializeField] private float fadeDuration = 5;

        public event Action OnHit;

        private FSM finiteStateMachine = new FSM();
        private HealthComp healthComponent = null;
        private float distanceToTarget = Mathf.Infinity;
        private float distanceToTargetSqr = Mathf.Infinity;
        private NavMeshAgent agent = null;
        private Animator animatorController = null;
        private Transform targetPointTransform = null;
        private float distanceToTargetPointTransform = Mathf.Infinity;
        private float distanceToTargetPointTransformSqr = Mathf.Infinity;
        private Transform currentTarget = null;
        private Weapon equippedWeapon;
        private bool isFrenzy = false;

        public HealthComp HealthComponent { get { return healthComponent; } }
        public float DistanceToTarget { get { return distanceToTarget; } }
        public NavMeshAgent Agent { get { return agent; } }
        public Animator AnimatorController { get { return animatorController; } }
        public Transform TargetPointTransform { get { return targetPointTransform; }}
        public Transform CurrentTarget { get { return currentTarget; } }
        public float ChaseSpeed { get { return chaseSpeed; } }
        public int BaseAttackDamage { get { return attackDamage; } }
        public float AttackRange { get { return attackRange; } }
        public float AttackInterval { get { return attackInterval; } }
        public float Accuracy { get { return accuracy; } }
        public float FrenzyRadius { get { return frenzyRadius; } }
        public float FrenzySpeed { get { return frenzySpeed; } }
        public bool IsFrenzy { get { return isFrenzy; } }
        public float DeathAnimationDuration { get { return deathAnimationDuration; } }
        public float FadeDuration { get { return fadeDuration; } }
        public Weapon EquippedWeapon { get { return equippedWeapon; } }
        public EnemyType Type { get { return type; } }
        public float Speed { get { return currentState == AIState.Chase ? ChaseSpeed : currentState == AIState.Frenzy ? frenzySpeed : 0; } }
        
        private static List<HealthComp> mouseTargets = new List<HealthComp>();
        private static List<HealthComp> catTargets = new List<HealthComp>();
        private static List<HealthComp> dogTargets = new List<HealthComp>();

        private static HealthComp[] allHealthCompsInScene;
        private Vector3 startPosition;

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

        private static bool active;
        private bool hasStarted;

        private void OnEnable()
        {
            if (agent)
            {
                startPosition = transform.position;
                agent.Warp(startPosition);
            }

            if (hasStarted)
                Start();

            active = true;
            HealthComp.OnCaravanDestroyed += OnCaravanDestroyedHandler;
            if (healthComponent)
                healthComponent.OnEnemyHealthChanged += OnHealthChangedHandler;
        }

        private void OnDisable()
        {
            HealthComp.OnCaravanDestroyed -= OnCaravanDestroyedHandler;
            if (healthComponent)
                healthComponent.OnEnemyHealthChanged -= OnHealthChangedHandler;
        }

        private void OnCaravanDestroyedHandler(HealthComp healthComp)
        {
            active = false;
        }

        private void OnHealthChangedHandler(HealthComp healthComp)
        {

        }

        private void Start()
        {
            hasStarted = true;

            Reset();
            PopulateTargetLists();

            animatorController = GetComponent<Animator>();
            healthComponent = GetComponent<HealthComp>();

            // override NavMeshAgent auto reposition when enabled
            SetPosition();

            GetWeaponReference();
            InitFSM();

            if (KickUpGrassR)
            {
                grassR = Instantiate(KickUpGrassR, transform.position, Quaternion.identity, null);
            }
           
            if (KickUpGrassL)
            {
                grassL = Instantiate(KickUpGrassL, transform.position, Quaternion.identity, null);
            }
        }

        private void SetPosition()
        {
            startPosition = transform.position;
            agent = GetComponent<NavMeshAgent>();
            agent.Warp(startPosition);
        }

        private void GetWeaponReference()
        {
            EnemyWeapon enemyWeapon = GetComponent<EnemyWeapon>();
            if (enemyWeapon)
            {
                enemyWeapon.Init();
                equippedWeapon = enemyWeapon.CurrentWeapon;
            }
        }

        private void Reset()
        {
            active = true;
            allHealthCompsInScene = null;
            mouseTargets.Clear();
            catTargets.Clear();
            dogTargets.Clear();
        }

        /// <summary>
        /// Get all targets for each type of enemy
        /// </summary>
        private void PopulateTargetLists()
        {
            if (allHealthCompsInScene == null)
                allHealthCompsInScene = FindObjectsOfType<HealthComp>();
            if (mouseTargets.Count == 0)
                mouseTargets = GetMouseTargets();
            if (catTargets.Count == 0)
                catTargets = GetCatTargets();
            if (dogTargets.Count == 0)
                dogTargets = GetDogTargets();
        }

        private void Update()
        {
            if (active)
                finiteStateMachine.Update(Time.deltaTime);

            if (currentTarget)
            {
                //distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);
                distanceToTarget = GetProjectedDistanceMagnitude(transform.position, currentTarget.position);
            }

            if (targetPointTransform)
            {
                //distanceToTargetPointTransform = Vector3.Distance(transform.position, targetPointTransform.position);
                distanceToTargetPointTransform = GetProjectedDistanceMagnitude(transform.position, targetPointTransform.position);
            }
        }

        /// <summary>
        /// Initiate State Machine by adding states and transitions between the states
        /// </summary>
        private void InitFSM()
        {
            // add states
            finiteStateMachine.AddState(AIState.FindTarget.ToString(), new FindTarget(this));
            finiteStateMachine.AddState(AIState.Chase.ToString(), new Chase(this));
            finiteStateMachine.AddState(AIState.Attack.ToString(), new Attack(this));
            finiteStateMachine.AddState(AIState.Frenzy.ToString(), new Frenzy(this));
            finiteStateMachine.AddState(AIState.PreAttack.ToString(), new PreAttack(this));
            finiteStateMachine.AddState(AIState.Death.ToString(), new Death(this));

            // add transitions
            finiteStateMachine.AddTransition(AIState.FindTarget.ToString(), AIState.Frenzy.ToString(), BaseConditionToFrenzy);
            finiteStateMachine.AddTransition(AIState.Chase.ToString(), AIState.Frenzy.ToString(), BaseConditionToFrenzy);
            finiteStateMachine.AddTransition(AIState.Attack.ToString(), AIState.Frenzy.ToString(), BaseConditionToFrenzy);

            finiteStateMachine.AddTransition(AIState.Chase.ToString(), AIState.PreAttack.ToString(), BaseConditionToPreAttack);

            finiteStateMachine.AddAnyStateTransition(AIState.FindTarget.ToString(), BaseConditionToFindTarget);
            finiteStateMachine.AddAnyStateTransition(AIState.Chase.ToString(), BaseConditionToChase);
            finiteStateMachine.AddAnyStateTransition(AIState.Attack.ToString(), BaseConditionToAttack);
            finiteStateMachine.AddAnyStateTransition(AIState.Death.ToString(), BaseConditionToDeath);
        }

        #region TRANSITIONS
        /// <summary>
        /// The condition to change from attack state to find target state
        /// </summary>
        private bool BaseConditionToFindTarget()
        {
            return !healthComponent.IsDead() && !isFrenzy && !currentTarget;
        }

        /// <summary>
        /// The condition to change from attack state to chase state
        /// </summary>
        private bool BaseConditionToChase()
        {
            return !healthComponent.IsDead() && !isFrenzy && currentTarget && distanceToTarget > attackRange;
        }

        /// <summary>
        /// The condition to change from find target state to attack state
        /// </summary>
        private bool BaseConditionToAttack()
        {
            return !healthComponent.IsDead() && !isFrenzy && currentTarget && distanceToTarget <= attackRange;
        }

        /// <summary>
        /// The condition to change from any state to frenzy state
        /// </summary>
        private bool BaseConditionToFrenzy()
        {
            return isFrenzy;
        }

        private bool BaseConditionToPreAttack()
        {
            return targetPointTransform && distanceToTargetPointTransform <= transitionDistanceTolerant;
        }

        private bool BaseConditionToDeath()
        {
            return healthComponent.IsDead();
        }
        #endregion

        /// <summary>
        /// Return a list of target depend on the enemy type
        /// </summary>
        public List<HealthComp> GetTargets()
        {
            switch (type)
            {
                case EnemyType.Mouse:
                    return mouseTargets;
                case EnemyType.Cat:
                    return catTargets;
                case EnemyType.Dog:
                    return dogTargets;
            }

            return null;
        }

        /// <summary>
        /// Get all targets for mouse
        /// </summary>
        private List<HealthComp> GetMouseTargets()
        {
            List<HealthComp> targets = new List<HealthComp>();

            for (int i = 0; i < allHealthCompsInScene.Length; i++)
            {
                if (allHealthCompsInScene[i].myClass == CharacterClass.Caravan)
                {
                    targets.Add(allHealthCompsInScene[i]);
                }
            }

            return targets;
        }

        /// <summary>
        /// Get all targets for cat
        /// </summary>
        private List<HealthComp> GetCatTargets()
        {
            List<HealthComp> targets = new List<HealthComp>();

            for (int i = 0; i < allHealthCompsInScene.Length; i++)
            {
                if (allHealthCompsInScene[i].myClass == CharacterClass.Player || allHealthCompsInScene[i].myClass == CharacterClass.Caravan)
                {
                    targets.Add(allHealthCompsInScene[i]);
                }
            }

            return targets;
        }

        /// <summary>
        /// Get all targets for dog
        /// </summary>
        private List<HealthComp> GetDogTargets()
        {
            List<HealthComp> targets = new List<HealthComp>();

            for (int i = 0; i < allHealthCompsInScene.Length; i++)
            {
                if (allHealthCompsInScene[i].myClass == CharacterClass.Player)
                {
                    targets.Add(allHealthCompsInScene[i]);
                }
            }

            return targets;
        }

        /// <summary>
        /// Set current target for AI
        /// </summary>
        /// <param name="target"> The target that will be set as current target </param>
        public void SetCurrentTarget(Transform target)
        {
            currentTarget = target;
        }

        /// <summary>
        /// Add a target to target list
        /// </summary>
        /// <param name="target"> The target to be added to list </param>
        public static void AddToTargetList(HealthComp target)
        {
            switch (target.myClass)
            {
                case CharacterClass.Player:
                    AddToCatTargets(target);
                    AddToDogTargets(target);
                    break;
                case CharacterClass.Caravan:
                    AddToMouseTargets(target);
                    AddToCatTargets(target);
                    break;
            }
        }

        private static void AddToMouseTargets(HealthComp target)
        {
            if (!mouseTargets.Contains(target))
                mouseTargets.Add(target);
        }

        private static void AddToCatTargets(HealthComp target)
        {
            if (!catTargets.Contains(target))
                catTargets.Add(target);
        }

        private static void AddToDogTargets(HealthComp target)
        {
            if (!dogTargets.Contains(target))
                dogTargets.Add(target);
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

        /// <summary>
        /// Remove a target from target list
        /// </summary>
        /// <param name="target"> The target to be removed from list </param>
        public static void RemoveFromTargetList(HealthComp target)
        {
            switch (target.myClass)
            {
                case CharacterClass.Player:
                    RemoveFromCatTargets(target);
                    RemoveFromDogTargets(target);
                    break;
                case CharacterClass.Caravan:
                    RemoveFromMouseTargets(target);
                    RemoveFromCatTargets(target);
                    break;
            }
        }

        private static void RemoveFromMouseTargets(HealthComp target)
        {
            if (mouseTargets.Contains(target))
                mouseTargets.Remove(target);
        }

        private static void RemoveFromCatTargets(HealthComp target)
        {
            if (catTargets.Contains(target))
                catTargets.Remove(target);
        }

        private static void RemoveFromDogTargets(HealthComp target)
        {
            if (dogTargets.Contains(target))
                dogTargets.Remove(target);
        }

        /// <summary>
        /// Set the temporary speed of AI agent with timer after which the speed will be restored
        /// </summary>
        /// <param name="speed"> The speed that will be set as AI agent speed </param>
        /// <param name="timer"></param>
        public void SetTemporaryMovementSpeed(float speed, float timer)
        {
            StartCoroutine(SetAndRestoreSpeed(speed, timer));
        }

        /// <summary>
        /// Set the temporary speed of AI agent with timer after which the speed will be restored
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="timer"></param>
        private IEnumerator SetAndRestoreSpeed(float speed, float timer)
        {
            float baseSpeed = 0;

            if (currentState == AIState.Chase)
            {
                baseSpeed = chaseSpeed;
                chaseSpeed = speed;
                agent.speed = speed;
                yield return new WaitForSeconds(timer);
                chaseSpeed = baseSpeed;
                agent.speed = chaseSpeed;
            }
            else if (currentState == AIState.Frenzy)
            {
                baseSpeed = frenzySpeed;
                frenzySpeed = speed;
                agent.speed = speed;
                yield return new WaitForSeconds(timer);
                frenzySpeed = baseSpeed;
                agent.speed = frenzySpeed;
            }
            else
            {
                yield return null;
            }
            if(GetComponent<Animator>() != null)
            {
                GetComponent<Animator>().SetFloat("Chase", agent.speed);
            }
        }

        /// <summary>
        /// Toogle the frenzy state on for a certain amount of time
        /// </summary>
        /// <param name="timer"> The time that AI will be set in frenzy state </param>
        public void ToggleFrenzyStateWithTimer(float timer)
        {
            StartCoroutine(SetAndRestoreFrenzyState(timer));
        }

        /// <summary>
        /// Toogle the frenzy state on for a certain amount of time
        /// </summary>
        /// <param name="timer"> The time that AI will be set in frenzy state </param>
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

            if (OnHit != null)
                OnHit.Invoke();
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

        public void SetTargetPoint(Transform pointTransform)
        {
            targetPointTransform = pointTransform;
        }

        public static void ResetTargets()
        {
            allHealthCompsInScene = null;
            mouseTargets.Clear();
            catTargets.Clear();
            dogTargets.Clear();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}