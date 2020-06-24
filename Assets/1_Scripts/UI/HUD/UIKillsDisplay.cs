using TMPro;
using UnityEngine;

public class UIKillsDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text textField;
    [SerializeField] private Fighter damageDealer;
    
    private int killCount;

    private void OnEnable()
    {
        if (damageDealer)
            damageDealer.OnEnemyKilled += UpdateKillUI;
    }

    private void UpdateKillUI(int killCount)
    {
        if (textField)
            textField.text = "Kills: " + killCount;
    }

    private void OnDisable()
    {
        if (damageDealer)
            damageDealer.OnEnemyKilled -= UpdateKillUI;
    }
}
