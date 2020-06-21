using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Catavaneer;

public class PlayerSelectController : MonoBehaviour
{
    public CharacterManager charman;
    public int PlayerID;
    public Image PlayerSelectReference;
    int SelectIndex;
    public List<Image> Selections = new List<Image>();
    public float selectdelay=1f;
    float timer=0;
    public string inputhorizontalaxis;
    public string inputacceptbutton;   //Submit/Interact
    public string inputbackbutton;
    public bool lockedin=false;
    public int totalplayers=0;
    public GameManager gman;
    // Start is called before the first frame update
    void Start()
    {
        gman = FindObjectOfType<GameManager>();
        charman = FindObjectOfType<CharacterManager>();
        SelectIndex = PlayerID;
       

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.T)) //Debug for if you only have one controller
        {
            
            foreach (PlayerSelectController player in FindObjectsOfType<PlayerSelectController>())
            {
                player.lockedin = true;
                PlayerSelectReference.color = Color.black;

            }
          //  StartCoroutine(gman.StartDelay());
            
            charman.StartGame();

        }


       
        if (!lockedin)
        {
            if (Input.GetButtonDown(inputacceptbutton))
            {
             
                lockedin = true;
                PlayerSelectReference.color = Color.black;
                CheckForReady();
                
            }
            else if (Input.GetAxis(inputhorizontalaxis) > 0 && Time.time > timer)
            {
                MoveRight();
                timer = Time.time + selectdelay;
            }
            else if (Input.GetAxis(inputhorizontalaxis) < 0 && Time.time > timer)
            {
                MoveLeft();
                timer = Time.time + selectdelay;
            }
        }
        
    }

    void MoveLeft()
    {
        SelectIndex--;
        if (SelectIndex < 0) //Wrap around if select index is less than 0.
        {
            SelectIndex = 3;
        }

        PlayerSelectReference.transform.position = new Vector3(Selections[SelectIndex].transform.position.x, PlayerSelectReference.transform.position.y, PlayerSelectReference.transform.position.z);
            
    }
    void MoveRight()
    {
        SelectIndex++;
        if (SelectIndex > 3) //Wrap around if select index is less than 0.
        {
            SelectIndex = 0;
        }
        PlayerSelectReference.transform.position = new Vector3(Selections[SelectIndex].transform.position.x, PlayerSelectReference.transform.position.y, PlayerSelectReference.transform.position.z);
    }

    void CheckForReady()
    {
        int playerslocked = 0;
        totalplayers = charman.playercount;
        //Debug.Log(charman.playercount);
        foreach (PlayerSelectController player in FindObjectsOfType<PlayerSelectController>())
        {
            if (player.lockedin)
            {
                if (player.SelectIndex == 0)
                {
                    charman.charnames[player.PlayerID]= "Russel";
                }
                else if (player.SelectIndex == 1)
                {
                    charman.charnames[player.PlayerID] = "Momo";
                }
                else if (player.SelectIndex == 2)
                {
                    charman.charnames[player.PlayerID] = "Kiki";
                }
                else if (player.SelectIndex == 3)
                {
                    charman.charnames[player.PlayerID] = "Jojo";
                }
                
                playerslocked++;
               
            }
           
        }
        
        if (playerslocked >= totalplayers)
        {
            // StartCoroutine(gman.StartDelay());
            //Debug.Log(totalplayers);
            //Debug.Log(playerslocked);
            charman.StartGame();
        }
    }
}
