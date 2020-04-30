﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPlot : MonoBehaviour
{
    bool isoccupied=false;
    bool ispurchased = false;
    public Shop shop;
    public PlayerInventory InvRef;
    public int plotid;


    private void Start()
    {
        shop = GetComponentInParent<Shop>();
    }

    private void OnTriggerEnter(Collider collision)
        {
        // create reference to the player interacting with shop
            if (!isoccupied)
            {


                if (collision.gameObject.GetComponent<PlayerInventory>())
                {
                InvRef = collision.gameObject.GetComponent<PlayerInventory>();
                InvRef.plotref = this;
                isoccupied = true;
            }
          
            }
        }

    private void OnTriggerExit(Collider collision)
    {
        //Removes reference when the shop is exited
           
            if (InvRef == collision.gameObject.GetComponent<PlayerInventory>())
            {
                InvRef.plotref = null;
                InvRef = null;
                isoccupied = false;
            }
  
    }

    public void CheckIfCanPurchase()
    {
        if (InvRef.gold >= shop.displayedItems[plotid].item_cost)
        {

            //check if player has inventory space for the item
            if (shop.displayedItems[plotid].type == Item.ItemType.Consumable)
            {
                if (InvRef.ConsumableItem == null)
                {
                    //remove item cost from player gold
                    InvRef.gold -= shop.displayedItems[plotid].item_cost;
                    //create a copy of the displayed item
                    Item purchaseditem = Instantiate(shop.displayedItems[plotid]);
                    //add item to player inventory
                    InvRef.ConsumableItem = purchaseditem;
                    //disable gameobjects for player copy and shop
                    InvRef.ConsumableItem.gameObject.SetActive(false);
                    shop.displayedItems[plotid].gameObject.SetActive(false);
                    ispurchased = true;
                }
                else
                {
                    Debug.Log("No space for consumable in inventory");
                }
            }
            else if (shop.displayedItems[plotid].type == Item.ItemType.Weapon)
            {
                if (InvRef.WeaponItem == null)
                {
                    //remove item cost from player gold
                    InvRef.gold -= shop.displayedItems[plotid].item_cost;
                    //create a copy of the displayed item
                    Item purchaseditem = Instantiate(shop.displayedItems[plotid]);
                    //add item to player inventory
                    InvRef.WeaponItem = purchaseditem;
                    //disable gameobjects for player copy and shop
                    InvRef.WeaponItem.gameObject.SetActive(false);
                    shop.displayedItems[plotid].gameObject.SetActive(false);
                    ispurchased = true;
                  
                }
                else
                {
                    Debug.Log("No space for weapon in inventory");
                }
            }
            else if (shop.displayedItems[plotid].type == Item.ItemType.Trap)
            {
                if (InvRef.TrapItem == null)
                {
                    //remove item cost from player gold
                    InvRef.gold -= shop.displayedItems[plotid].item_cost;
                    //create a copy of the displayed item
                    Item purchaseditem = Instantiate(shop.displayedItems[plotid]);
                    //add item to player inventory
                    InvRef.TrapItem = purchaseditem;
                    //disable gameobjects for player copy and shop
                    InvRef.TrapItem.gameObject.SetActive(false);
                    shop.displayedItems[plotid].gameObject.SetActive(false);
                    ispurchased = true;
                }
                else
                {
                    Debug.Log("No space for trap in inventory");
                }
            }
            else
            {
                Debug.LogError("Invalid Inventory Item Type");
            }
            

        }
        else
        {
            Debug.Log("Not enough gold");
        }
       
    }
   

}
