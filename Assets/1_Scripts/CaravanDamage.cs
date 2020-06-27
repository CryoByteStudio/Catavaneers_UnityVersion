using Catavaneer;
using System.Collections.Generic;
using UnityEngine;

public class CaravanDamage : MonoBehaviour
{
    public List<GameObject> damageparticles =new List<GameObject>();
    public GameObject FinalParticle;
    public GameObject VictoryParticles;
    public int damageStage = 0;

    private void OnEnable()
    {
        if (GameManager.Instance)
            GameManager.OnLevelComplete += PlayWinFX;
    }

    private void OnDisable()
    {
        if (GameManager.Instance)
            GameManager.OnLevelComplete -= PlayWinFX;
    }

    private void PlayWinFX()
    {
        PlayVictory();
    }

    public void TriggerDamageStageParticles()
    {
        if(damageparticles[damageStage] != null)
        {
            damageparticles[damageStage].SetActive(true);
            damageStage++;
            // TODO Add more partical
        } 
    }

    public void ReverseDamageStageParticles()
    {
        if (damageparticles[damageStage] != null)
        {
            damageStage--;
            if (damageStage < 0)
            {
                damageStage = 0;
            }
            damageparticles[damageStage].SetActive(false);
            
            // TODO Add more partical
        }
    }

    public void ResetDamage()
    {
        foreach(GameObject particle in damageparticles)
        {
            particle.SetActive(false);
            
        }
        damageStage = 0;
    }

    public void PlayVictory()
    {
        foreach (GameObject particle in damageparticles)
        {
            particle.gameObject.SetActive(false);
        }

        VictoryParticles.SetActive(true);
    }

    public void TriggerFinalDamageStageParticle()
    {
        FinalParticle.SetActive(true);
        //damageStage++;
    }

    public void FlingPart(int currenthealth)
    {
        
    }
}
