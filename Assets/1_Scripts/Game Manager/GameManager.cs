using ObjectPooling;
using SpawnSystem;
using System;
using System.Collections;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    public DifficultyLevel diff;
    public float startDelay = 1;
    public float quitDelay = 0;
    //public GameDifficultyManager difman;
    private bool doneOnce = false;

    [SerializeField] HealthComp caravan_HC;

    // Make this the one instance managing pooled objects throughout levels
    #region SINGLETON
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }


    private void Awake()
    {
        if (instance)
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

    private void Update()
    {
      //  if (Input.GetKeyDown(KeyCode.C)) { difficulty = DifficultyLevel.IronCat; } 
    
        if (!caravan_HC)
        {
            HealthComp[] healthComps = FindObjectsOfType<HealthComp>();
            foreach (HealthComp hc in healthComps)
            {
                if (hc.myClass == CharacterClass.Caravan)
                {
                    caravan_HC = hc;
                    break;
                }
            }
            Debug.Log("NO Caravan attached to game manager");
        }

        if (caravan_HC)
        {
            // always start coroutine once in update
            if (caravan_HC.IsDead() && !doneOnce)
            {
                doneOnce = true;
                caravan_HC.SetIsDead(false);

                StartCoroutine(RestartLevel());

                transform.MyExtensionFunction();
            }
        }

        if (Input.anyKeyDown && SceneManager.GetActiveScene().name == "Menu_Credits")
        {
            SceneManager.LoadScene("Menu_Main");
        }

        if(Input.GetKeyDown(KeyCode.JoystickButton6))
        {
            ScreenCapture.CaptureScreenshot("ScreenShot_" + System.DateTime.Now.ToString("yyyy-MM-dd-HH_mm_ss") + ".JPG");
        }
    }

    private IEnumerator RestartLevel()
    {
        ObjectPooler.DisableAllActiveObjects();
        yield return new WaitForSeconds(quitDelay);
        //string curScene = SceneManager.GetActiveScene().name;
        Reset();
        SceneManager.LoadScene("Menu_LoseScene");
        //StartCoroutine(StartDelay());
    }

    private void Reset()
    {
        doneOnce = false;
    }

    public  IEnumerator StartDelay()
    {
        SpawnManager.CanSpawn = false;
        yield return new WaitForSeconds(startDelay);
        SpawnManager.CanSpawn = true;
    }

    private IEnumerator QuitDelay()
    {
        yield return new WaitForSeconds(quitDelay);
        QuitGame();
    }

    private static void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }


    //below section edit by Will
    public void ToMainMenu()
    {
        doneOnce = true;
        if (caravan_HC != null) caravan_HC.SetIsDead(false);
        ObjectPooler.DisableAllActiveObjects();
        SceneManager.LoadScene(0);
       if(FindObjectOfType<CharacterManager>())
        {
            Destroy(FindObjectOfType<CharacterManager>().gameObject);
            
        }
    }

    public void ToPlayerSelectionScene()
    {
        doneOnce = true;
        if(caravan_HC != null) caravan_HC.SetIsDead(false);
        ObjectPooler.DisableAllActiveObjects();
        SceneManager.LoadScene("Menu_CharacterSelect");
    }

    public void LoadLevel(string leveltoload)
    {
        Debug.Log("Loading: " + leveltoload);
        SceneManager.LoadScene(leveltoload);
    }

    public void StartSceneButton()
    {
       
     
        SceneManager.LoadScene("Menu_CharacterSelect");
        Debug.Log("normal");
        FindObjectOfType<GameDifficultyManager>().dif = DifficultyLevel.Normal;
    }
    public void StartIroncatButton()
    {
   
        SceneManager.LoadScene("Menu_CharacterSelect");
        Debug.Log("ironcat");
        FindObjectOfType<GameDifficultyManager>().dif = DifficultyLevel.IronCat;

    }
    public void StartCatfightButton()
    {

        SceneManager.LoadScene("Catfight_01");
        Debug.Log("Catfight");
        FindObjectOfType<GameDifficultyManager>().dif = DifficultyLevel.Catfight;

    }
    public void StartCatpocalypseButton()
    {
   
        SceneManager.LoadScene("Menu_CharacterSelect");
        Debug.Log("catpoc");
        FindObjectOfType<GameDifficultyManager>().dif=DifficultyLevel.Catapocalypse;
    }

    public void CreditsSceneButton()
    {
        LoadLevel("Menu_Credits");
    }

    public void QuitApp()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
}
