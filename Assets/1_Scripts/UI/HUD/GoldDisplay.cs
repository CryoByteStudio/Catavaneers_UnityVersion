using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text textField;
    [SerializeField] private PlayerInventory playerInventoryRef;
    
    private void OnEnable()
    {
        if (playerInventoryRef)
            playerInventoryRef.OnGoldChanged += UpdateGoldUI;
    }

    private void UpdateGoldUI()
    {
        if (playerInventoryRef && textField)
            textField.text = "Gold: " + playerInventoryRef.Gold;
    }

    private void OnDisable()
    {
        if (playerInventoryRef)
            playerInventoryRef.OnGoldChanged -= UpdateGoldUI;
    }
}
