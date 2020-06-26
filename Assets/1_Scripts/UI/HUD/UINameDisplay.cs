using UnityEngine;
using TMPro;
using System.Linq;

public class UINameDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text textField;
    [SerializeField] private PlayerInfo playerInfoRef;

    private string characterName;

    private void Start()
    {
        if (CharacterManager.Instance)
        {
            characterName = CharacterManager.Instance.charNames.ElementAtOrDefault(playerInfoRef.PlayerID);

            if (textField && playerInfoRef && !string.IsNullOrEmpty(characterName))
                textField.text = CharacterManager.Instance.charNames[playerInfoRef.PlayerID] + " [P" + (playerInfoRef.PlayerID + 1) + "]";
            else
                textField.text = "[Player: " + playerInfoRef.PlayerID + 1 + "]";
        }
    }
}
