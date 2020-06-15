using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerSelector : MonoBehaviour
{
    public List<GameObject> spawners= new List<GameObject>();
    CharacterManager cman;
    // Start is called before the first frame update
    void Start()
    {
       cman = FindObjectOfType<CharacterManager>();

       
        if (cman)
        {
            switch (cman.playercount)
            {
                case 1:
                    spawners[0].gameObject.SetActive(true);
                    Destroy(spawners[1].gameObject);
                    Destroy(spawners[2].gameObject);
                    Destroy(spawners[3].gameObject);
                    break;
                case 2:
                    spawners[1].gameObject.SetActive(true);
                    Destroy(spawners[0].gameObject);
                    Destroy(spawners[2].gameObject);
                    Destroy(spawners[3].gameObject);
                    break;
                case 3:
                    spawners[2].gameObject.SetActive(true);
                    Destroy(spawners[1].gameObject);
                    Destroy(spawners[0].gameObject);
                    Destroy(spawners[3].gameObject);
                    break;
                case 4:
                    spawners[3].gameObject.SetActive(true);
                    Destroy(spawners[1].gameObject);
                    Destroy(spawners[2].gameObject);
                    Destroy(spawners[0].gameObject);
                    break;
                default:
                    Debug.LogWarning("Invalid player number on" + cman.name+ " Defaulting to spawner 1");
                 
                    spawners[0].gameObject.SetActive(true);
                    Destroy(spawners[1].gameObject);
                    Destroy(spawners[2].gameObject);
                    Destroy(spawners[3].gameObject);
                    break;
            }


            

        }
        else
        {
            Debug.LogWarning("No character manager found  Defaulting to spawner 1");
            spawners[0].gameObject.SetActive(true);
            Destroy(spawners[1].gameObject);
            Destroy(spawners[2].gameObject);
            Destroy(spawners[3].gameObject); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
