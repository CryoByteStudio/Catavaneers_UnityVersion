using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CustomMathLibrary;
using System;

public enum CharacterNames
{
    None,
    Russell,
    Jojo,
    Kiki,
    Momo
}

public class PlayerSelectController : MonoBehaviour
{
    public int PlayerID;
    public Image playerSelectReference;
    private int selectIndex;
    public List<Image> selections = new List<Image>();
    public float selectDelay = 1f;
    public float loadDelay = 5f;
    public float startDelay = 3f;
    private float timer = 0;
    public string inputHorizontalAxis;
    public string inputAcceptButton;   //Submit/Interact
    public bool lockedIn = false;
    public int totalPlayers = 0;
    public bool LockedIn => lockedIn;

    private CharacterNames chosenCharacter = CharacterNames.None;
    private PlayerSelectController[] playerSelectControllers;
    private MenuCharacterAnimation[] menuCharacterAnimations;

    private void Start()
    {
        timer = Time.time + startDelay;
        selectIndex = PlayerID;
        playerSelectControllers = FindObjectsOfType<PlayerSelectController>();
        menuCharacterAnimations = FindObjectsOfType<MenuCharacterAnimation>();
    }

    private void Update()
    {
        if (!lockedIn && Time.time > startDelay)
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

                switch (selections[player.selectIndex].name)
                {
                    case "Russell":
                        MusicManager.Instance.PlaySoundTrack(SoundClipsInts.RussellCharSelect);
                        chosenCharacter = CharacterNames.Russell;
                        break;
                    case "Jojo":
                        MusicManager.Instance.PlaySoundTrack(SoundClipsInts.JojoCharSelect);
                        chosenCharacter = CharacterNames.Jojo;
                        break;
                    case "Kiki":
                        MusicManager.Instance.PlaySoundTrack(SoundClipsInts.KikiCharSelect);
                        chosenCharacter = CharacterNames.Kiki;
                        break;
                    case "Momo":
                        MusicManager.Instance.PlaySoundTrack(SoundClipsInts.MomoCharSelect);
                        chosenCharacter = CharacterNames.Momo;
                        break;
                    default:
                        break;
                }

                MakeCatDance();

                playersLocked++;
            }
        }

        if (playersLocked >= totalPlayers)
        {
            StartCoroutine(StartGame());
        }
    }

    private void MakeCatDance()
    {
        foreach (var anim in menuCharacterAnimations)
        {
            if (anim.CharacterName == chosenCharacter)
                anim.Dance();
        }
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(loadDelay);
        CharacterManager.Instance.StartGame();
    }
}
