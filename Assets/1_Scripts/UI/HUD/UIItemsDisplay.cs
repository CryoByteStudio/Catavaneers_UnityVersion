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
    [SerializeField] private TrapSystem trapSystemRef;

    private Sprite defaultSprite;

    private void Awake()
    {
        defaultSprite = weaponImage.sprite;
    }

    private void OnEnable()
    {
        if (playerInventoryRef)
        {
            playerInventoryRef.OnWeaponChanged += UpdateWeaponUI;
            playerInventoryRef.OnConsumableChanged += UpdateConsumableUI;
        }

        if (trapSystemRef)
        {
            trapSystemRef.OnTrap1Updated += UpdateTrap1UI;
            trapSystemRef.OnTrap2Updated += UpdateTrap2UI;
        }
    }

    private void UpdateWeaponUI(Sprite sprite)
    {
        if (weaponImage)
        {
            weaponImage.sprite = sprite;
            return;
        }

        weaponImage.sprite = defaultSprite;
    }

    private void UpdateTrap1UI(Sprite sprite)
    {
        if (trap1Image)
        {
            trap1Image.sprite = sprite;
            return;
        }

        trap1Image.sprite = defaultSprite;
    }

    private void UpdateTrap2UI(Sprite sprite)
    {
        if (trap2Image)
        {
            trap2Image.sprite = sprite;
            return;
        }

        trap2Image.sprite = defaultSprite;
    }

    private void UpdateConsumableUI(Sprite sprite)
    {
        if (consumableImage)
        {
            consumableImage.sprite = sprite;
            return;
        }

        consumableImage.sprite = defaultSprite;
    }

    private void OnDisable()
    {
        if (playerInventoryRef)
        {
            playerInventoryRef.OnWeaponChanged -= UpdateWeaponUI;
            playerInventoryRef.OnConsumableChanged -= UpdateConsumableUI;
        }

        if (trapSystemRef)
        {
            trapSystemRef.OnTrap1Updated -= UpdateTrap1UI;
            trapSystemRef.OnTrap2Updated -= UpdateTrap2UI;
        }
    }
}
