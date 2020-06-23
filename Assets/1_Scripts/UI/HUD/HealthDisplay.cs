using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text textField;
    [SerializeField] private Slider healthBar;
    [SerializeField] private HealthComp playerHealthCompRef;

    private bool hasSetUpHealthBar;

    private void OnEnable()
    {
        if (playerHealthCompRef)
            playerHealthCompRef.OnPlayerHealthChanged += UpdateHealthUI;
    }

    private void SetUpHealthBar()
    {
        if (playerHealthCompRef && healthBar)
        {
            healthBar.minValue = 0;
            healthBar.maxValue = playerHealthCompRef.startHealth;
        }
    }

    private void UpdateHealthUI()
    {
        if (!hasSetUpHealthBar)
        {
            SetUpHealthBar();
            hasSetUpHealthBar = true;
        }

        if (playerHealthCompRef)
        {
            if (textField)
                textField.text = playerHealthCompRef.GetCurHealth() + "/" + playerHealthCompRef.startHealth;
            if (healthBar)
                healthBar.value = playerHealthCompRef.GetCurHealth();
        }
    }

    private void OnDisable()
    {
        if (playerHealthCompRef)
            playerHealthCompRef.OnPlayerHealthChanged -= UpdateHealthUI;
    }
}
