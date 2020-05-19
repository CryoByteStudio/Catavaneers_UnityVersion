using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{

  
    public List<string> charnames = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
       
        DontDestroyOnLoad(this);
        charnames.Add(default);
        charnames.Add(default);
        charnames.Add(default);
        charnames.Add(default);


    }

   
}

  




