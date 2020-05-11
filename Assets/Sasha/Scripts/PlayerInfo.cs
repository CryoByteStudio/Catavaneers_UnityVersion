using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public int PlayerID;

    public SkinnedMeshRenderer rendref;


    public Material Jojoskin;
    public Material Momoskin;
    public Material Kikiskin;
    public Material Russellskin;
    public CharacterManager charman;
    // Start is called before the first frame update
    void Start()
    {
        charman=FindObjectOfType<CharacterManager>();
        if (charman.charnames[PlayerID] == "Russell")
        {
            rendref.material = Russellskin;

        }else if (charman.charnames[PlayerID] == "Momo") 
        {
            rendref.material = Momoskin;

        }
        if (charman.charnames[PlayerID] == "Kiki")
        {
            rendref.material = Kikiskin;

        }
        if (charman.charnames[PlayerID] == "Jojo")
        {
            rendref.material = Jojoskin;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
