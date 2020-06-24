using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemsDisplay : MonoBehaviour
{
    [SerializeField] private Image weaponImage;
    [SerializeField] private Image trap1Image;
    [SerializeField] private Image trap2Image;
    [SerializeField] private Image consumableImage;
    [SerializeField] private PlayerInventory playerInventoryRef;

    private void OnEnable()
    {
        if (playerInventoryRef)
        {
            playerInventoryRef.OnWeaponChanged += UpdateWeaponUI;
            playerInventoryRef.OnTrap1Changed += UpdateTrap1UI;
            playerInventoryRef.OnTrap2Changed += UpdateTrap2UI;
            playerInventoryRef.OnConsumableChanged += UpdateConsumableUI;
        }
    }

    private void UpdateWeaponUI(Sprite sprite)
    {
        if (weaponImage)
            weaponImage.sprite = sprite;
    }

    private void UpdateTrap1UI(Sprite sprite)
    {
        if (trap1Image)
            trap1Image.sprite = sprite;
    }

    private void UpdateTrap2UI(Sprite sprite)
    {
        if (trap2Image)
            trap2Image.sprite = sprite;
    }

    private void UpdateConsumableUI(Sprite sprite)
    {
        if (consumableImage)
            consumableImage.sprite = sprite;
    }

    private void OnDisable()
    {
        if (playerInventoryRef)
        {
            playerInventoryRef.OnWeaponChanged -= UpdateWeaponUI;
            playerInventoryRef.OnTrap1Changed -= UpdateTrap1UI;
            playerInventoryRef.OnTrap2Changed -= UpdateTrap2UI;
            playerInventoryRef.OnConsumableChanged -= UpdateConsumableUI;
        }
    }
}
