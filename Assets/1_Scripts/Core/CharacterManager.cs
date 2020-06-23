using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Catavaneer.Singleton;
using Catavaneer.MenuSystem;

public class CharacterManager : SingletonEntity<CharacterManager>
{
    public int LastEncounterIndex=0;
    public int CurrentDay = 0;

    [SerializeField] private GameObject[] PlayerIcons;

    bool isstarted = false;
    public int playercount;

    public List<string> charnames = new List<string>();
    
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        
        for(int i = 0; i < Input.GetJoystickNames().Length; i++)
        {
            PlayerIcons[i].SetActive(true);
        }
    }
    
    void Start()
    {
        playercount = Input.GetJoystickNames().Length;
        Debug.Log(playercount + " Players connected)");
        charnames.Add(default);
        charnames.Add(default);
        charnames.Add(default);
        charnames.Add(default);
        Debug.Log("Start");
    }

    protected override void OnEnable()
    {
        Catavaneer.LevelManagement.LevelLoader.OnSceneLoaded += OnSceneLoadedHandler;
    }

    private void OnSceneLoadedHandler(Scene arg0, LoadSceneMode arg1)
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
        Catavaneer.LevelManagement.LevelLoader.OnSceneLoaded -= OnSceneLoadedHandler;
    }

    public void StartGame()
    {
        if (!isstarted)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            MenuManager.OpenGameMenu();
            isstarted = true;
        }
    }
}