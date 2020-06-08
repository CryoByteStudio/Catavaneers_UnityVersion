using Catavaneer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Goldbag : MonoBehaviour
{

    public PlayerInventory holdersInventory;
    public bool isHeld = false;
    public int goldpertick = 10;
     float goldticktimer = 0;
    public int victorygold = 100;
    public float timebetweengoldtick = 0.5f;
    public float droptime;
    float droptimer=0f;

    public GameObject victoryui;
    public Text victorytext;

    // Start is called before the first frame update
    void Start()
    {

    }


    private void OnTriggerEnter(Collider collision)
    {
        
        if (!isHeld)
        {
            if (Time.time > droptimer) {
                if (collision.gameObject.GetComponent<PlayerInventory>())
                {
                    transform.parent = collision.gameObject.transform;
                    holdersInventory = collision.gameObject.GetComponent<PlayerInventory>();
                    isHeld = true;
                }
            }
        }
        
    }

        public void DropBag()
        {
            transform.parent = null;
            isHeld = false;
            droptimer = Time.time + droptime;
        }
        // Update is called once per frame
        void Update()
        {

        if (victoryui.activeInHierarchy == false)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                DropBag();
            }

            if (isHeld)
            {
                if (Time.time >= goldticktimer + timebetweengoldtick)
                {
                    holdersInventory.gold += goldpertick;
                    goldticktimer = Time.time;

                    if (holdersInventory.gold >= victorygold)
                    {
                        victoryui.SetActive(true);
                        victorytext.text = "Congratulations! " + holdersInventory.playername + " wins! press any key to return to the main menu.";
                    }
                }
            }
        }
        else
        {
            if (Input.anyKeyDown)
            {
               //FindObjectOfType<GameManager>().ToMainMenu();
                SceneManager.LoadScene("SplashScreen");
            }
        }
        }
    } 
