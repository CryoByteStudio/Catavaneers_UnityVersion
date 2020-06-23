using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Catavaneer;
using CustomMathLibrary;

public class PlayerSelectController : MonoBehaviour
{
    public int PlayerID;
    public Image playerSelectReference;
    private int selectIndex;
    public List<Image> selections = new List<Image>();
    public float selectDelay = 1f;
    private float timer = 0;
    public string inputHorizontalAxis;
    public string inputAcceptButton;   //Submit/Interact
    //public string inputBackButton;
    public bool lockedIn = false;
    public int totalPlayers = 0;
    //public CharacterManager charman;
    //public GameManager gman;
    public bool LockedIn => lockedIn;

    private PlayerSelectController[] playerSelectControllers;

    private void Start()
    {
        //gman = FindObjectOfType<GameManager>();
        //charman = FindObjectOfType<CharacterManager>();
        selectIndex = PlayerID;
        playerSelectControllers = FindObjectsOfType<PlayerSelectController>();
    }

    private void LateUpdate()
    {
        //if (Input.GetKeyDown(KeyCode.T)) //Debug for if you only have one controller
        //{
            
        //    foreach (PlayerSelectController player in FindObjectsOfType<PlayerSelectController>())
        //    {
        //        player.lockedin = true;
        //        playerSelectReference.color = Color.black;

        //    }
        //    // StartCoroutine(gman.StartDelay());
            
        //    charman.StartGame();
        //}

        if (!lockedIn)
        {
            if (Input.GetButtonDown(inputAcceptButton))
            {
                lockedIn = true;
                playerSelectReference.color = Color.black;
                CheckForReady();
            }
            else if (Input.GetAxis(inputHorizontalAxis) > 0 && Time.time > timer)
            {
                MoveRight();
                timer = Time.time + selectDelay;
            }
            else if (Input.GetAxis(inputHorizontalAxis) < 0 && Time.time > timer)
            {
                MoveLeft();
                timer = Time.time + selectDelay;
            }
        }

        if (Input.GetKey(KeyCode.C))
        {
            lockedIn = false;
            playerSelectReference.color = Color.white;
        }
    }

    private void MoveLeft()
    {
        selectIndex = CustomMathf.GetPreviousLoopIndex(selectIndex, selections.Count);

        playerSelectReference.transform.position = new Vector3(selections[selectIndex].transform.position.x, playerSelectReference.transform.position.y, playerSelectReference.transform.position.z);
            
    }

    private void MoveRight()
    {
        selectIndex = CustomMathf.GetNextLoopIndex(selectIndex, selections.Count);

        playerSelectReference.transform.position = new Vector3(selections[selectIndex].transform.position.x, playerSelectReference.transform.position.y, playerSelectReference.transform.position.z);
    }

    private void CheckForReady()
    {
        if (!CharacterManager.Instance)
        {
            Debug.Log("Character Manager is null");
            return;
        }

        int playersLocked = 0;
        //totalPlayers = charman.playerCount;
        totalPlayers = CharacterManager.Instance.playerCount;
        //Debug.Log(charman.playercount);
        foreach (PlayerSelectController player in playerSelectControllers)
        {
            if (player.lockedIn)
            {
                //if (player.selectIndex == 0)
                //{
                //    //charman.charNames[player.PlayerID]= "Russel";
                //    CharacterManager.Instance.charNames[player.PlayerID] = "Russel";
                //}
                //else if (player.selectIndex == 1)
                //{
                //    //charman.charNames[player.PlayerID] = "Jojo";
                //    CharacterManager.Instance.charNames[player.PlayerID] = "Jojo";
                //}
                //else if (player.selectIndex == 2)
                //{
                //    //charman.charNames[player.PlayerID] = "Kiki";
                //    CharacterManager.Instance.charNames[player.PlayerID] = "Kiki";
                //}
                //else if (player.selectIndex == 3)
                //{
                //    //charman.charNames[player.PlayerID] = "Momo";
                //    CharacterManager.Instance.charNames[player.PlayerID] = "Momo";
                //}

                CharacterManager.Instance.charNames[player.PlayerID] = selections[player.selectIndex].name;
                Debug.Log(selections[player.selectIndex].name);
                
                playersLocked++;
            }
        }
        
        if (playersLocked >= totalPlayers)
        {
            //charman.StartGame();
            CharacterManager.Instance.StartGame();
        }
    }
}
