using System.Collections.Generic;
using AI.States;

namespace AI
{
    public class DogAIController : AIController
    {
        private static List<HealthComp> targets = new List<HealthComp>();
        public static List<HealthComp> Targets => targets;

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        protected override void Start()
        {
            base.Start();
            targets = GetAllTargets();
        }

        protected override void Reset()
        {
            base.Reset();
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void RegisterToEvents()
        {
            base.RegisterToEvents();
        }

        protected override void UnregisterToEvents()
        {
            base.UnregisterToEvents();
        }

        protected override void InitializeFSM()
        {
            // add states
            finiteStateMachine.AddState(AIState.FindTarget.ToString(), new DogFindTarget(this));
            finiteStateMachine.AddState(AIState.Chase.ToString(), new DogChase(this));
            finiteStateMachine.AddState(AIState.Attack.ToString(), new DogAttack(this));
            finiteStateMachine.AddState(AIState.Frenzy.ToString(), new DogFrenzy(this));
            finiteStateMachine.AddState(AIState.PreAttack.ToString(), new DogPreAttack(this));
            finiteStateMachine.AddState(AIState.Death.ToString(), new DogDeath(this));

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
        protected bool BaseConditionToFindTarget()
        {
            return !healthComponent.IsDead() && !isFrenzy && !currentTarget;
        }

        protected bool BaseConditionToChase()
        {
            return !healthComponent.IsDead() && !isFrenzy && currentTarget && distanceToTarget > attackRange;
        }

        protected bool BaseConditionToAttack()
        {
            return !healthComponent.IsDead() && !isFrenzy && currentTarget && distanceToTarget <= attackRange;
        }

        protected bool BaseConditionToFrenzy()
        {
            return isFrenzy;
        }

        protected bool BaseConditionToPreAttack()
        {
            return targetPointTransform && distanceToTargetPointTransform <= transitionDistanceTolerant;
        }

        protected bool BaseConditionToDeath()
        {
            return healthComponent.IsDead();
        }
        #endregion

        private static List<HealthComp> GetAllTargets()
        {
            List<HealthComp> targets = new List<HealthComp>();

            for (int i = 0; i < allTargetsWithHealthComponent.Length; i++)
            {
                if (allTargetsWithHealthComponent[i].myClass == CharacterClass.Player)
                {
                    targets.Add(allTargetsWithHealthComponent[i]);
                }
            }

            return targets;
        }

        public static void AddToTargetsList(HealthComp target)
        {
            if (!targets.Contains(target))
                targets.Add(target);
        }

        public static void RemoveFromTargetList(HealthComp target)
        {
            if (targets.Contains(target))
                targets.Remove(target);
        }
    }
}