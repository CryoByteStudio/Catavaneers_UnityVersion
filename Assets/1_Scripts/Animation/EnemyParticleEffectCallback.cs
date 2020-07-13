using AI;
using UnityEngine;

public class EnemyParticleEffectCallback : MonoBehaviour
{
    public ParticleSystem kickUpGrassFXLeft;
    public ParticleSystem kickUpGrassFXRight;
    public ParticleSystem hitEffect;
    public ParticleSystem freezeEffect;
    public ParticleSystem damageEffect;
    public ParticleSystem slowEffect;
    public ParticleSystem reverseEffect;

    private AIController ai;

    private void Start()
    {
        ai = GetComponent<AIController>();
    }

    private void Hit()
    {
        ai.InvokeOnHit();
    }

    private void FootL()
    {
        if (kickUpGrassFXLeft)
        {
            kickUpGrassFXLeft.Stop();
            kickUpGrassFXLeft.Play();
        }
    }

    private void FootR()
    {
        if (kickUpGrassFXRight)
        {
            kickUpGrassFXRight.Stop();
            kickUpGrassFXRight.Play();
        }
    }
}
