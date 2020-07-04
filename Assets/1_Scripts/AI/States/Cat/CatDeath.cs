using FiniteStateMachine.StatePolymorphism;
using ObjectPooling;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace AI.States
{
    public class CatDeath : State
    {
        private CatAIController controller = null;
        private Animator animatorController = null;
        private HealthComp healthComponent = null;
        private NavMeshAgent agent = null;
        private Vector3 destination = Vector3.zero;
        private Collider collider = null;
        private CharacterFader rendererFader = null;
        private float deathAnimationDuration = 0;
        private float fadeDuration = 0;

        public CatDeath(CatAIController controller)
        {
            this.controller = controller;
        }

        override public void OnStateEnter()
        {
            Init();
            DeathBehaviour();
            controller.currentState = AIState.Death;
        }

        override public void Update(float deltaTime)
        {
        }

        override public void OnStateExit()
        {
        }

        private void Init()
        {
            if (!controller)
                Debug.LogWarning("Controller is not set in Patrol state");
            if (!agent)
                agent = controller.Agent;
            if (!animatorController)
                animatorController = controller.AnimatorController;
            if (!healthComponent)
                healthComponent = controller.HealthComponent;
            if (!collider)
                collider = controller.GetComponent<Collider>();
            if (!rendererFader)
                rendererFader = controller.transform.GetComponent<CharacterFader>();

            deathAnimationDuration = controller.DeathAnimationDuration;
            fadeDuration = controller.FadeDuration;
        }

        private void DeathBehaviour()
        {
            StopMoving();
            controller.StartCoroutine(DeathBehaviourRoutine());
        }

        private void StopMoving()
        {
            controller.SetCurrentTarget(null);
            destination = controller.transform.position;
            agent.SetDestination(destination);
            agent.isStopped = true;
        }

        private IEnumerator DeathBehaviourRoutine()
        {
            animatorController.SetTrigger("Die");
            if (collider)
                collider.enabled = false;
            yield return new WaitForSeconds(deathAnimationDuration);
            FadeOut();
            yield return new WaitForSeconds(fadeDuration);
            healthComponent.ResetHealthComp();
            ObjectPooler.SetInactive(controller.gameObject);
            ResetFade();
        }

        private void FadeOut()
        {
            if (rendererFader)
                rendererFader.FadeOut(deathAnimationDuration + 0.1f);
        }

        private void ResetFade()
        {
            if (rendererFader)
                rendererFader.ResetFade();
            if (collider)
                collider.enabled = true;
        }
    }
}