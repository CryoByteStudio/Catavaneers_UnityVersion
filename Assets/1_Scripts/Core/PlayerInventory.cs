using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerInventory : MonoBehaviour //Sasha
{
    public SoundClipsInts soundCue = SoundClipsInts.Buying;
    public string playername;
    public int playerindex;
    public int kills=0;
    [SerializeField] private int startGold=1000;
    private int currentGold;
    public int Gold => currentGold;
    public Item WeaponItem;
    public Item ConsumableItem;
    public Item TrapItem;
    public ShopPlot plotref;

    //ui
    //public Text NameUI;
    public TMP_Text NameUI;
    public Image Trap1UI;
    public Image Trap2UI;
    public Image ConsumableUI;
    public Item startingWeaponItem;
    //public CharacterManager cman;
    public string buyButton;
    public string useItemButton;
    public string cancelButton;

    public GameObject PlayerUIRef;

    public event Action OnGoldChanged;
    public event Action<Sprite> OnConsumableChanged;
    public event Action<Sprite> OnWeaponChanged;
    public event Action<Sprite> OnTrap1Changed;
    public event Action<Sprite> OnTrap2Changed;

    private void Start()
    {
        AddGold(startGold);

        //cman= FindObjectOfType<CharacterManager>();
        //if (cman)
        if (CharacterManager.Instance)
        {
            //if (cman.playerCount <= playerindex)
            if (CharacterManager.Instance.playerCount <= playerindex)
            {
                gameObject.SetActive(false);
                PlayerUIRef.gameObject.SetActive(false);
            }
        }

        if (startingWeaponItem)
        {
            EquipStartingWeapon();
        }
        else
        {
            Debug.LogWarning("No starting weapon equipped to: "+name);
        }
    }

    public void EquipStartingWeapon()
    {
        GetComponent<Fighter>().EquipWeapon(startingWeaponItem.WeaponRef);

        WeaponItem = startingWeaponItem;
        if (WeaponItem)
        {
            if (OnWeaponChanged != null)
                OnWeaponChanged.Invoke(WeaponItem.Item_Display);
        }
        WeaponItem = null;
    }

    private void Update()
    {
        //#TODO Move these inputs to the actual player controller
        if (Input.GetButtonDown(buyButton))
        {
            if (plotref)
            {
                plotref.CheckIfCanPurchase();

                if (TrapItem)
                {
                    GetComponent<TrapSystem>().EquipTrap(TrapItem.TrapRef);
                    int trapno = GetComponent<TrapSystem>().CheckHasTrap();

                    switch (trapno)
                    {
                        case 0:
                            Debug.Log("no traps");
                            MusicManager.Instance.PlaySoundTrack(soundCue);
                            break;
                        case 1:
                            Debug.Log("Trap1");
                            //Trap1UI.sprite = TrapItem.Item_Display;
                            if (OnTrap1Changed != null)
                                OnTrap1Changed.Invoke(TrapItem.Item_Display);
                            MusicManager.Instance.PlaySoundTrack(soundCue);
                            break;
                        case 2:
                            Debug.Log("Only Trap 2");
                            MusicManager.Instance.PlaySoundTrack(soundCue);
                            break;
                        case 3:
                            Debug.Log("both traps");
                            //Trap2UI.sprite = TrapItem.Item_Display;
                            if (OnTrap2Changed != null)
                                OnTrap2Changed.Invoke(TrapItem.Item_Display);
                            break;
                    }
                    
                    TrapItem = null;
                    //Destroy(TrapItem);
                }
                if (WeaponItem)
                {
                    GetComponent<Fighter>().EquipWeapon(WeaponItem.WeaponRef);
                    //WeaponUI.sprite = WeaponItem.Item_Display;
                    if (OnWeaponChanged != null)
                        OnWeaponChanged.Invoke(WeaponItem.Item_Display);
                    WeaponItem = null;
                    MusicManager.Instance.PlaySoundTrack(soundCue);

                    //Destroy(WeaponItem);
                }
                if (ConsumableItem)
                {
                    //ConsumableUI.sprite = ConsumableItem.Item_Display;
                    if (OnConsumableChanged != null)
                        OnConsumableChanged.Invoke(ConsumableItem.Item_Display);
                    MusicManager.Instance.PlaySoundTrack(soundCue);
                }
            }
            else
            {
                Debug.Log("Tried to purchase but no plot reference");
            }
        }
        else if (Input.GetButtonDown(useItemButton))
        {
            if (ConsumableItem)
            {
                Debug.Log("Use Consumable");
                GetComponent<HealthComp>().AddHealth(25);
                //ConsumableUI.sprite = null;
                if (OnConsumableChanged != null)
                    OnConsumableChanged.Invoke(null);
                Destroy(ConsumableItem.gameObject);
            }
            else
            {
                Debug.Log("No Consumable in inventory");
            }
        }
        
        else if (Input.GetButtonDown(cancelButton))
        {
            if (TrapItem)
            {
                int trapno = GetComponent<TrapSystem>().CheckHasTrap();
                Debug.Log("Use Trap");
                switch (trapno)
                {
                    case 0:
                        Debug.Log("no traps");
                        //Trap1UI = null;
                        //Trap2UI = null;
                        if (OnTrap1Changed != null)
                            OnTrap1Changed.Invoke(null);
                        if (OnTrap2Changed != null)
                            OnTrap2Changed.Invoke(null);
                        break;
                    case 1:
                        Debug.Log("Trap1");
                        //Trap2UI = null;
                        if (OnTrap1Changed != null)
                            OnTrap1Changed.Invoke(null);
                        break;
                    case 2:
                        Debug.Log("Only Trap 2");
                        //Trap1UI = null;
                        if (OnTrap2Changed != null)
                            OnTrap2Changed.Invoke(null);
                        break;
                    case 3:
                        Debug.Log("both traps");
                        break;
                }

                Destroy(TrapItem.gameObject);
            }
            else
            {
                Debug.Log("No trap in inventory");
            }
        }

       
        /*   Test Controller in shop test scene
        else if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(Camera.main.transform.right * -20f);
        }else if (Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(Camera.main.transform.up * 20f);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            
            rb.AddForce(Camera.main.transform.right * 20f);
        }*/
    }

    public void RemoveGoldFromInventory(float percentageGoldToKeep)
    {
        currentGold = Mathf.RoundToInt(currentGold * percentageGoldToKeep);

        if (OnGoldChanged != null)
            OnGoldChanged.Invoke();
    }

    public void AddGold(int amount)
    {
        currentGold += amount;

        if (OnGoldChanged != null)
            OnGoldChanged.Invoke();
    }
}
