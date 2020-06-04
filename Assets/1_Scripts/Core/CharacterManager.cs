using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Catavaneer.Singleton;

public class CharacterManager : SingletonEntity<CharacterManager>
{
    bool isstarted = false;
    public int playercount;

    public List<string> charnames = new List<string>();
    
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
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

    protected override void OnDisable()
    {
        Catavaneer.LevelManagement.LevelLoader.OnSceneLoaded -= OnSceneLoadedHandler;
    }

    public void StartGame()
    {
        if (!isstarted)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            isstarted = true;
        }
    }
}