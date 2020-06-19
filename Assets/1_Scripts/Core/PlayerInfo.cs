using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public int PlayerID;

    public SkinnedMeshRenderer rendref;


    public Material P1Skin;
    public Material P2Skin;
    public Material P3Skin;
    public Material P4Skin;
    public CharacterManager charman;
    // Start is called before the first frame update
    void Start()
    {
        charman=FindObjectOfType<CharacterManager>();
        //Debug.Log("startingitup");
        if (charman)
        {
            //Debug.Log("charactermans");
            if (charman.charnames[PlayerID] == "Russel")
            {
                Debug.Log("Setting rusel skin");
                rendref.material = P1Skin;

            }
            else if (charman.charnames[PlayerID] == "Momo")
            {
                rendref.material = P2Skin;

            }
            if (charman.charnames[PlayerID] == "Kiki")
            {
                rendref.material = P3Skin;

            }
            if (charman.charnames[PlayerID] == "Jojo")
            {
                rendref.material = P4Skin;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
