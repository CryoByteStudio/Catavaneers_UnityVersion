using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDImageDisplay : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private PlayerInfo playerInfoRef;
    private int playerID;

    private void Start()
    {
        if (playerInfoRef)
            playerID = playerInfoRef.PlayerID;

        AssignBackgrounImage();
    }

    private void AssignBackgrounImage()
    {
        if (CharacterManager.Instance.charNames == null && CharacterManager.Instance.charNames.Count <= 0)
            return;

        if (playerID >= CharacterManager.Instance.charNames.Count)
            return;

        if (CharacterManager.Instance && backgroundImage)
        {
            foreach (PlayerData playerData in CharacterManager.Instance.PlayerData)
            {
                if (CharacterManager.Instance.charNames[playerID] == playerData.name)
                {
                    backgroundImage.sprite = playerData.uiSprite;
                    break;
                }
            }
        }
    }
}
