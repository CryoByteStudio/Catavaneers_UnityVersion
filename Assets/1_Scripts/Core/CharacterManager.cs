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
    //public int LastEncounterIndex=0;
    //public int CurrentDay = 0;
    [SerializeField] private int firstWorkingSceneIndex;
    [SerializeField] private List<PlayerData> playerData;
    public List<PlayerData> PlayerData => playerData;

    bool isstarted = false;
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
        Debug.Log(playerCount + " Players connected)");
        //charNames.Add(default);
        //charNames.Add(default);
        //charNames.Add(default);
        //charNames.Add(default);
        for (int i = 0; i < maxPlayerCount; i++)
        {
            charNames.Add(default);
        }
        Debug.Log("Start");
    }

    protected override void OnEnable()
    {
        LevelLoader.OnSceneLoaded += OnSceneLoadedHandler;
    }

    private void OnSceneLoadedHandler(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.LoadSceneMode arg1)
    {
        isstarted = false;
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level == 1)
        {
            Destroy(this.gameObject);
        }
    }

    protected override void OnDisable()
    {
        LevelLoader.OnSceneLoaded -= OnSceneLoadedHandler;
    }

    public void StartGame()
    {
        if (!isstarted)
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            MenuManager.LoadCampaignLevel();
            MenuManager.OpenGameMenu();
            isstarted = true;
        }
    }
}