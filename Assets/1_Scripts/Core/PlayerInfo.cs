using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    public int PlayerID;
    public SkinnedMeshRenderer rendref;
    public Image playerimageref;
    public Material P1Skin;
    public Material P2Skin;
    public Material P3Skin;
    public Material P4Skin;
    public Sprite Russelsprite;
    public Sprite Kikisprite;
    public Sprite MomoSprite;
    public Sprite JojoSprite;
    public CharacterManager charman;

    void Start()
    {
        charman=FindObjectOfType<CharacterManager>();

        if (charman)
        {
            //Debug.Log("charactermans");
            //if (charman.charnames[PlayerID] == "Russel")
            //{
            //    Debug.Log("Setting rusel skin");
            //    rendref.material = P1Skin;
            //}
            //else if (charman.charnames[PlayerID] == "Momo")
            //{
            //    rendref.material = P2Skin;
            //}
            //if (charman.charnames[PlayerID] == "Kiki")
            //{
            //    rendref.material = P3Skin;
            //}
            //if (charman.charnames[PlayerID] == "Jojo")
            //{
            //    rendref.material = P4Skin;
            //}

            if (charman.charnames == null && charman.charnames.Count <= 0)
            {
                Debug.LogError("CharacterManager's charnams is null or emty");
                return;
            }

            foreach (string name in charman.charnames)
            {
                switch (name)
                {
                    case "Russel": 
                        rendref.material = P1Skin;
                        playerimageref.sprite = Russelsprite;
                        break;
                    case "Momo": 
                        rendref.material = P2Skin;
                        playerimageref.sprite = Kikisprite;
                        break;
                    case "Kiki":
                        rendref.material = P3Skin;
                        playerimageref.sprite = MomoSprite;
                        break;
                    case "Jojo": 
                        rendref.material = P4Skin;
                        playerimageref.sprite = JojoSprite;
                        break;
                    default: break;
                }
            }
        }
    }
}
