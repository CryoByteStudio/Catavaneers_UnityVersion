using AI;
using UnityEngine;

public class ParticleEffectCallback : MonoBehaviour
{
    public ParticleSystem KickUpGrassL;
    public ParticleSystem KickUpGrassR;

    public ParticleSystem hitEffect;
    public ParticleSystem grassR;
    public ParticleSystem grassL;

    public ParticleSystem FreezeEffect;
    public ParticleSystem DamageEffect;
    public ParticleSystem SlowEffect;
    public ParticleSystem ReverseEffect;

    private AIController ai;

    private void Start()
    {
        ai = GetComponent<AIController>();

        if (KickUpGrassR)
        {
            grassR = Instantiate(KickUpGrassR, transform.position, Quaternion.identity, null);
        }

        if (KickUpGrassL)
        {
            grassL = Instantiate(KickUpGrassL, transform.position, Quaternion.identity, null);
        }
    }

    private void Hit()
    {
        ai.InvokeOnHit();
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
    }
}
