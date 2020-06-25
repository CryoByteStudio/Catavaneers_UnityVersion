using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Catavaneer.Singleton;
using Catavaneer.MenuSystem;
using Catavaneer.LevelManagement;

[System.Serializable]
public struct PlayerData
{
    public string name;
    public Material originalMaterial;
    public Material swapMaterial;
    public Sprite uiSprite;
}

public class CharacterManager : SingletonEntity<CharacterManager>
{
    [SerializeField] private int firstWorkingSceneIndex;
    [SerializeField] private List<GameObject> playerSelectorIcons;
    [SerializeField] private List<PlayerData> playerData;
    public List<PlayerData> PlayerData => playerData;

    private bool hasStarted = false;
    public int playerCount;
    public int maxPlayerCount;

    public List<string> charNames = new List<string>();
    
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
    
    void Start()
    {
        playerCount = Input.GetJoystickNames().Length;
        Debug.Log(playerCount + " players connected.");

        if (playerSelectorIcons != null && playerSelectorIcons.Count >= playerCount)
        {
            for (int i = 0; i < playerCount; i++)
            {
                playerSelectorIcons[i].SetActive(true);
            }
        }
        
        for (int i = 0; i < maxPlayerCount; i++)
        {
            charNames.Add(default);
        }
    }

    protected override void OnEnable()
    {
        LevelLoader.OnSceneLoaded += OnSceneLoadedHandler;
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level == 1)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnSceneLoadedHandler(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.LoadSceneMode arg1)
    {
        hasStarted = false;
    }

    protected override void OnDisable()
    {
        LevelLoader.OnSceneLoaded -= OnSceneLoadedHandler;
    }

    public void StartGame()
    {
        if (!hasStarted)
        {
            MenuManager.LoadCampaignLevel();
            MenuManager.OpenGameMenu();
            hasStarted = true;
        }
    }
}