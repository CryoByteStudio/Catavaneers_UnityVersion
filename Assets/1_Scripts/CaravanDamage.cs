using System.Collections;
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
        
           
                damageparticles[damageStage].SetActive(true);
                damageStage++;
                
            
        
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
