using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour
{
    bool isstarted = false;
    public int playercount;

    public List<string> charnames = new List<string>();

    // Start is called before the first frame update
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

    public void StartGame()
    {
        if (!isstarted)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            isstarted = true;

           

        }
    }

 
}


   

  




