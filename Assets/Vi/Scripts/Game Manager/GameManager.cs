using ObjectPooling;
using System.Collections;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float startDelay = 1;
    public float quitDelay = 0;
    private bool doneOnce = false;
    [SerializeField] HealthComp caravan_HC;

    // Make this the one instance managing pooled objects throughout levels
    #region SINGLETON
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }


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
        StartCoroutine(StartDelay());

        //if (!caravan_HC)
        //{
        //    HealthComp[] healthComps = FindObjectsOfType<HealthComp>();
        //    foreach(HealthComp hc in healthComps)
        //    {
        //        if (hc.myClass == CharacterClass.Caravan)
        //        {
        //            caravan_HC = hc;
        //            break;
        //        }
        //    }
        //    Debug.Log("NO Caravan attached to game manager");
        //}
    }

    private void Update()
    {
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
        //else if (caravan_HC.IsDead())
        //{
        //    caravan_HC = null;
        //    ObjectPooler.DisableAllActiveObjects();
        //    StartCoroutine("RestartLevel");
        //}

        if (caravan_HC)
        {
            // always start coroutine once in update
            if (caravan_HC.IsDead() && !doneOnce)
            {
                doneOnce = true;
                caravan_HC.SetIsDead(false);

                StartCoroutine("RestartLevel");
            }
        }

        //if (SpawnManager.HasFinishedSpawning && SpawnManager.EnemiesAlive <= 0 && !doneOnce)
        //{
        //    StartCoroutine(QuitDelay());
        //    doneOnce = true;
        //}

        if (Input.anyKeyDown && SceneManager.GetActiveScene().name == "Credits")
        {
            SceneManager.LoadScene("Menu_Main");
        }
    }

    private IEnumerator RestartLevel()
    {
        ObjectPooler.DisableAllActiveObjects();
        yield return new WaitForSeconds(startDelay);
        string curScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(5);
        //StartCoroutine(StartDelay());
    }

    private IEnumerator StartDelay()
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
    }

    public void ToPlayerSelectionScene()
    {
        doneOnce = true;
        if(caravan_HC != null) caravan_HC.SetIsDead(false);
        ObjectPooler.DisableAllActiveObjects();
        SceneManager.LoadScene(1);
    }

    public void LoadLevel(string leveltoload)
    {
        Debug.Log("Loading: " + leveltoload);
        SceneManager.LoadScene(leveltoload);
    }

    public void StartSceneButton()
    {
        SceneManager.LoadScene(1);
    }

    public void CreditsSceneButton()
    {
        LoadLevel("Credits");
    }

    public void QuitApp()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
}
