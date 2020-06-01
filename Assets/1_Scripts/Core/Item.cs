using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        Default,
        Weapon,
        Trap,
        Consumable
    };
    public Sprite Item_Display;
    public string item_name;
    public int item_cost;
    public Weapon WeaponRef;
    public TrapScriptable TrapRef;
    public ItemType type;

    

}
