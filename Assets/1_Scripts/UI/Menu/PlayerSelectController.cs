using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public bool lockedIn = false;
    public int totalPlayers = 0;
    public bool LockedIn => lockedIn;


    private PlayerSelectController[] playerSelectControllers;

    private void Start()
    {
        selectIndex = PlayerID;
        playerSelectControllers = FindObjectsOfType<PlayerSelectController>();
    }

    private void Update()
    {
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
        totalPlayers = CharacterManager.Instance.playerCount;
        foreach (PlayerSelectController player in playerSelectControllers)
        {
            if (player.lockedIn)
            {
                CharacterManager.Instance.charNames[player.PlayerID] = selections[player.selectIndex].name;
                Debug.Log(selections[player.selectIndex].name);

                if (selections[player.selectIndex].name == "Russell") MusicManager.Instance.PlaySoundTrack(SoundClipsInts.RussellCharSelect);
                if (selections[player.selectIndex].name == "Jojo") MusicManager.Instance.PlaySoundTrack(SoundClipsInts.JojoCharSelect);
                if (selections[player.selectIndex].name == "Kiki") MusicManager.Instance.PlaySoundTrack(SoundClipsInts.KikiCharSelect);
                if (selections[player.selectIndex].name == "Momo") MusicManager.Instance.PlaySoundTrack(SoundClipsInts.MomoCharSelect);

                playersLocked++;
            }
        }

        if (playersLocked >= totalPlayers)
        {
            StartCoroutine(StartGame());
        }
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(3);
        CharacterManager.Instance.StartGame();
    }
}
