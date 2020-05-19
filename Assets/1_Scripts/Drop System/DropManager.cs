using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DropItemType
{
    Coin,
    Weapon,
    HealthPack
}

[System.Serializable]
public class DropItem
{
    public DropItemType itemType;
    public List<GameObject> variants = new List<GameObject>();
}

public class DropManager : MonoBehaviour
{
    [SerializeField] private List<DropItem> dropItems = new List<DropItem>();
    private static Dictionary<DropItemType, List<GameObject>> dropDictionary = new Dictionary<DropItemType, List<GameObject>>();
    
    // Make this the one instance managing pooled objects throughout levels
    #region SINGLETON
    private static DropManager instance;
    public static DropManager Instance { get { return instance; } }


    private void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

    private void Start()
    {
        MakeDropDictionary();
    }

    private void MakeDropDictionary()
    {
        for (int i = 0; i < dropItems.Count; i++)
        {
            if (!dropDictionary.ContainsKey(dropItems[i].itemType))
                dropDictionary.Add(dropItems[i].itemType, dropItems[i].variants);
        }
    }

    public static void DropItem(DropItemType dropItem, Vector3 position)
    {
        Instantiate(dropDictionary[dropItem][Random.Range(0, dropDictionary[dropItem].Count)], position, Quaternion.identity);
    }
}
