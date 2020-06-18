using Catavaneer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Catavaneer.MenuSystem;

public class PauseManager : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject PauseMenuUI;
    public GameObject SettingMenuUI;
    public GameObject FirstOhject;
    public string pauseButton;
    // Start is called before the first frame update
    void Start()
    {
    }
    
   // Update is called once per frame
   void Update()
   {
       if(Input.GetButtonDown(pauseButton) || Input.GetKeyDown(KeyCode.Escape))
        {

       
            if(GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        SettingMenuUI.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(FirstOhject, null);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        //if (FindObjectOfType<GameManager>())
        //{
        //    FindObjectOfType<GameManager>().ToMainMenu();
        //}
        //else
        //{
        //    MenuManager.LoadMainMenuLevel(false);
        //}
     
        MenuManager.LoadMainMenuLevel(false);
    }
}
