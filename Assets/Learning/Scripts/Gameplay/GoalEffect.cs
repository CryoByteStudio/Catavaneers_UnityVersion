using System.Collections;
using UnityEngine;
using Learning.Utils;

namespace Learning.Gameplay
{
    public class GoalEffect : MonoBehaviour
    {
        [SerializeField] private Transform particleFX;
        [SerializeField] private float delay;

        private ParticleSystem[] particleSystems;

        #region UNITY ENGINE FUNCTION

        private void Start()
        {
            if (particleFX)
            {
                particleSystems = particleFX.GetComponentsInChildren<ParticleSystem>();
            }
            else
            {
                EditorHelper.LogError(this, "Particle system is null!");
            }
        }

        #endregion

        #region PRIVATE METHODS

        private IEnumerator PlayEffectRoutine()
        {
            yield return new WaitForSeconds(delay);

            if (particleFX && particleSystems == null)
            {
                particleSystems = particleFX.GetComponentsInChildren<ParticleSystem>();
            }

            if (particleSystems != null)
            {
                foreach (ParticleSystem ps in particleSystems)
                {
                    if (ps)
                    {
                        ps.Stop();
                        ps.Play();
                    }
                }
            }
        }

        #endregion

        #region PUBLIC METHODS

        public void PlayEffect()
        {
            StartCoroutine(PlayEffectRoutine());
        }

        #endregion

        #region PRIVATE STATIC METHODS
        #endregion

        #region PUBLIC STATIC METHODS
        #endregion
    }
}