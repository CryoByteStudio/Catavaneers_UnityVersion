using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class MainMenuController: MonoBehaviour
{
    public Color buttonBaseColor;
    public Color buttonSelectColor;

    public Image QuitButton;
    public Image StartButton;
    public Image CreditsButton;

    public Image SelectedButton;
    private void Start()
    {
        SelectedButton = StartButton;
        StartButton.color = buttonSelectColor;
        QuitButton.color = buttonBaseColor;
        CreditsButton.color = buttonBaseColor;
        //select start button
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
           
            ShiftSelectionDown();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
           
            ShiftSelectionUp();
        }
        else if (Input.GetKeyDown(KeyCode.Return)){
          
            UseButton();
        }
    }

    public void ShiftSelectionDown()
    {
        if (SelectedButton == StartButton)
        {
            StartButton.color = buttonBaseColor;
            CreditsButton.color = buttonSelectColor;
            SelectedButton = CreditsButton;
        }
        else if (SelectedButton == CreditsButton)
        {
            CreditsButton.color = buttonBaseColor;
            QuitButton.color = buttonSelectColor;
            SelectedButton = QuitButton;
        }
        else if (SelectedButton == QuitButton)
        {
            QuitButton.color = buttonBaseColor;
            StartButton.color = buttonSelectColor;
            SelectedButton = StartButton;
        }
    }
    public void ShiftSelectionUp()
    {
        if (SelectedButton == StartButton)
        {
            StartButton.color = buttonBaseColor;
            QuitButton.color = buttonSelectColor;
            SelectedButton = QuitButton;
        }
        else if (SelectedButton == CreditsButton)
        {
            CreditsButton.color = buttonBaseColor;
            StartButton.color = buttonSelectColor;
            SelectedButton = StartButton;
        }
        else if (SelectedButton == QuitButton)
        {
            QuitButton.color = buttonBaseColor;
            CreditsButton.color = buttonSelectColor;
            SelectedButton = CreditsButton;
           
        }
    }
    public void UseButton()
    {
        if (SelectedButton == StartButton)
        {
            
            LoadLevel("Sasha Test");
        }
        else if (SelectedButton == CreditsButton)
        {
           
            LoadLevel("Credits");
        }
        else if (SelectedButton == QuitButton)
        {
            
            QuitApp();  
        }
    }

    public void LoadLevel(string leveltoload)
    {
        Debug.Log("Loading: " + leveltoload);
        SceneManager.LoadScene(leveltoload);
    }

   
    public void QuitApp()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }

    //below section edit by Will

    public void StartSceneButton()
    {
        LoadLevel("Charselect");
    }

    public void CreditsSceneButton()
    {
        LoadLevel("Credits");
    }
}
