using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShopPlot : MonoBehaviour
{
    bool isoccupied=false;
    public bool ispurchased = false;
    public Shop shop;
    public PlayerInventory InvRef;
    public int plotid;
    public Text CostDisplay;
    public Image SoldOut;
    public Image ItemDisplay;

    public bool issoldout;

    private void Start()
    {
        shop = GetComponentInParent<Shop>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        // create reference to the player interacting with shop
        if (collision.gameObject.tag == "Player")
        {
            if (!isoccupied)
            {
                if (collision.gameObject.GetComponent<PlayerInventory>())
                {
                    InvRef = collision.gameObject.GetComponent<PlayerInventory>();
                    InvRef.plotref = this;
                    isoccupied = true;
                    ItemDisplay.gameObject.SetActive(true);
                    if (ispurchased)
                    {
                        SoldOut.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        //Removes reference when the shop is exited

        if (InvRef == collision.gameObject.GetComponent<PlayerInventory>() && collision.gameObject.tag == "Player")
        {
            InvRef.plotref = null;
            InvRef = null;
            isoccupied = false;
            ItemDisplay.gameObject.SetActive(false);
            SoldOut.gameObject.SetActive(false);
        }
    }

    public void CheckIfCanPurchase()
    {
        if (!issoldout)
        {
            if (InvRef.Gold >= shop.displayedItems[plotid].item_cost)
            {
                //check if player has inventory space for the item
                if (shop.displayedItems[plotid].type == Item.ItemType.Consumable)
                {
                    if (InvRef.ConsumableItem == null)
                    {
                        //remove item cost from player gold
                        InvRef.AddGold(-shop.displayedItems[plotid].item_cost);
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
                        InvRef.AddGold(-shop.displayedItems[plotid].item_cost);
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
                        InvRef.AddGold(-shop.displayedItems[plotid].item_cost);
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
        if (ispurchased)
        {
            SoldOut.gameObject.SetActive(true);
            issoldout = true;
        }
    }

    public void CleanUnusedTrap()
    {

    }
}
