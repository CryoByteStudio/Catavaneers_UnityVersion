using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FiniteStateMachine.StatePolymorphism;
using AI.States;
using System.Collections;

namespace AI
{
    public enum AIState
    {
        FindTarget,
        Chase,
        PreAttack,
        Attack,
        Frenzy
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

        [Header("Attack Settings")]
        [SerializeField] private int attackDamage = 0;
        [SerializeField] private float attackRange = 0;
        [SerializeField] private float attackInterval = 0;

        [Header("Frenzy Settings")]
        [SerializeField] private float frenzyRadius = 0;
        [SerializeField] private float frenzySpeed = 0;

        private FSM finiteStateMachine = new FSM();
        private float distanceToTarget = Mathf.Infinity;
        private NavMeshAgent agent = null;
        private Transform targetPointTransform = null;
        private Transform currentTarget = null;
        [SerializeField] private bool isFrenzy = false;

        public float DistanceToTarget { get { return distanceToTarget; } }
        public NavMeshAgent Agent { get { return agent; } }
        public Transform CurrentTarget { get { return currentTarget; } }
        public float ChaseSpeed { get { return chaseSpeed; } }
        public int AttackDamage { get { return attackDamage; } }
        public float AttackRange { get { return attackRange; } }
        public float AttackInterval { get { return attackInterval; } }
        public float FrenzyRadius { get { return frenzyRadius; } }
        public float FrenzySpeed { get { return frenzySpeed; } }
        public bool IsFrenzy { get { return isFrenzy; } }
        public EnemyType Type { get { return type; } }
        public float Speed { get { return currentState == AIState.Chase ? ChaseSpeed : currentState == AIState.Frenzy ? frenzySpeed : 0; } }

        private static List<HealthComp> mouseTargets = new List<HealthComp>();
        private static List<HealthComp> catTargets = new List<HealthComp>();
        private static List<HealthComp> dogTargets = new List<HealthComp>();

        private static HealthComp[] healthComps;
        private Vector3 startPosition;

        public AIState currentState = AIState.FindTarget;

        private void OnEnable()
        {
            if (agent)
            {
                startPosition = transform.position;
                agent.Warp(startPosition);
            }
        }

        private void Start()
        {
            PopulateTargetLists();

            // override NavMeshAgent auto reposition when enabled
            startPosition = transform.position;
            agent = GetComponent<NavMeshAgent>();
            agent.Warp(startPosition);

            InitFSM();
        }

        /// <summary>
        /// Get all targets for each type of enemy
        /// </summary>
        private void PopulateTargetLists()
        {
            if (healthComps == null)
                healthComps = FindObjectsOfType<HealthComp>();
            if (mouseTargets.Count == 0)
                mouseTargets = GetMouseTargets();
            if (catTargets.Count == 0)
                catTargets = GetCatTargets();
            if (dogTargets.Count == 0)
                dogTargets = GetDogTargets();
        }

        private void Update()
        {
            finiteStateMachine.Update(Time.deltaTime);

            if (currentTarget)
            {
                distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);
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

            // add transitions
            finiteStateMachine.AddTransition(AIState.FindTarget.ToString(), AIState.Frenzy.ToString(), BaseConditionToFrenzy);
            finiteStateMachine.AddTransition(AIState.Chase.ToString(), AIState.Frenzy.ToString(), BaseConditionToFrenzy);
            finiteStateMachine.AddTransition(AIState.Attack.ToString(), AIState.Frenzy.ToString(), BaseConditionToFrenzy);

            finiteStateMachine.AddTransition(AIState.Chase.ToString(), AIState.PreAttack.ToString(), BaseConditionToPreAttack);

            finiteStateMachine.AddAnyStateTransition(AIState.FindTarget.ToString(), BaseConditionToFindTarget);
            finiteStateMachine.AddAnyStateTransition(AIState.Chase.ToString(), BaseConditionToChase);
            finiteStateMachine.AddAnyStateTransition(AIState.Attack.ToString(), BaseConditionToAttack);
        }

        #region TRANSITIONS
        /// <summary>
        /// The condition to change from attack state to find target state
        /// </summary>
        private bool BaseConditionToFindTarget()
        {
            return !isFrenzy && !currentTarget;
        }

        /// <summary>
        /// The condition to change from attack state to chase state
        /// </summary>
        private bool BaseConditionToChase()
        {
            return !isFrenzy && currentTarget && distanceToTarget > attackRange;
        }

        /// <summary>
        /// The condition to change from find target state to attack state
        /// </summary>
        private bool BaseConditionToAttack()
        {
            return !isFrenzy && currentTarget && distanceToTarget <= attackRange;
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
            return targetPointTransform && Vector3.Distance(transform.position, targetPointTransform.position) <= 0.5f;
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

            for (int i = 0; i < healthComps.Length; i++)
            {
                if (healthComps[i].myClass == CharacterClass.Caravan)
                {
                    targets.Add(healthComps[i]);
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

            for (int i = 0; i < healthComps.Length; i++)
            {
                if (healthComps[i].myClass == CharacterClass.Player || healthComps[i].myClass == CharacterClass.Caravan)
                {
                    targets.Add(healthComps[i]);
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

            for (int i = 0; i < healthComps.Length; i++)
            {
                if (healthComps[i].myClass == CharacterClass.Player)
                {
                    targets.Add(healthComps[i]);
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
        public void AddToTargetList(HealthComp target)
        {
            switch (type)
            {
                case EnemyType.Mouse:
                    if (!mouseTargets.Contains(target))
                        mouseTargets.Add(target);
                    break;
                case EnemyType.Cat:
                    if (!catTargets.Contains(target))
                        catTargets.Add(target);
                    break;
                case EnemyType.Dog:
                    if (!dogTargets.Contains(target))
                        dogTargets.Add(target);
                    break;
            }
        }

        /// <summary>
        /// Remove a target from target list
        /// </summary>
        /// <param name="target"> The target to be removed from list </param>
        public void RemoveFromTargetList(HealthComp target)
        {
            switch (type)
            {
                case EnemyType.Mouse:
                    if (mouseTargets.Contains(target))
                        mouseTargets.Remove(target);
                    break;
                case EnemyType.Cat:
                    if (catTargets.Contains(target))
                        catTargets.Remove(target);
                    break;
                case EnemyType.Dog:
                    if (dogTargets.Contains(target))
                        dogTargets.Remove(target);
                    break;
            }
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

        public void SetTargetPoint(Transform pointTransform)
        {
            targetPointTransform = pointTransform;
        }

        public static void ResetTargets()
        {
            healthComps = null;
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