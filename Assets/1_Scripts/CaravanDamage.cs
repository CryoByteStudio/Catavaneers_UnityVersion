﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaravanDamage : MonoBehaviour
{
    public List<GameObject> damageparticles =new List<GameObject>();
    public GameObject FinalParticle;
    public int damageStage=0;
    // Start is called before the first frame update

  
    
    public void TriggerDamageStageParticles()
    {
        
        if(damageparticles[damageStage] != null)
        {
            damageparticles[damageStage].SetActive(true);
            damageStage++;
            // TODO Add more partical
        }
                
            
        
    }
    public void TriggerFinalDamageStageParticle()
    {


        FinalParticle.SetActive(true);
       // damageStage++;



    }

    public void FlingPart(int currenthealth)
    {
        
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.G))
        {
            TriggerDamageStageParticles();
        }*/
    }
}
