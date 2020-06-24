using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text textField;
    [SerializeField] private Slider healthBar;
    [SerializeField] private CharacterClass characterClass;
    [SerializeField] private HealthComp healthCompRef;

    private bool hasSetUpHealthBar = false;

    private void OnEnable()
    {
        if (healthCompRef)
        {
            switch (characterClass)
            {
                case CharacterClass.Player:
                    healthCompRef.OnPlayerHealthChanged += UpdateHealthUI;
                    break;
                case CharacterClass.Enemy:
                    healthCompRef.OnEnemyHealthChanged += UpdateHealthUI;
                    break;
                case CharacterClass.Caravan:
                    healthCompRef.OnCaravanHealthChanged += UpdateHealthUI;
                    break;
                case CharacterClass.Obj:
                default:
                    break;
            }
        }
    }

    private void SetUpHealthBar()
    {
        if (healthCompRef && healthBar)
        {
            healthBar.minValue = 0;
            healthBar.maxValue = healthCompRef.startHealth;
        }
    }

    private void UpdateHealthUI()
    {
        if (!hasSetUpHealthBar)
        {
            SetUpHealthBar();
            hasSetUpHealthBar = true;
        }

        if (healthCompRef)
        {
            if (textField)
                textField.text = healthCompRef.GetCurHealth() + "/" + healthCompRef.startHealth;
            if (healthBar)
                healthBar.value = healthCompRef.GetCurHealth();
        }
    }

    private void OnDisable()
    {
        if (healthCompRef)
        {
            switch (characterClass)
            {
                case CharacterClass.Player:
                    healthCompRef.OnPlayerHealthChanged += UpdateHealthUI;
                    break;
                case CharacterClass.Enemy:
                    healthCompRef.OnEnemyHealthChanged += UpdateHealthUI;
                    break;
                case CharacterClass.Caravan:
                    healthCompRef.OnCaravanHealthChanged += UpdateHealthUI;
                    break;
                case CharacterClass.Obj:
                default:
                    break;
            }
        }
    }
}
