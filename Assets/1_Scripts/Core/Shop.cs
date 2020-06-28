using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    
    float restockTimer=5f;
    [SerializeField]
    float timeToRestock=5f;

    public List<Item> availableItems=new List<Item>();
    public List<Item> displayedItems=new List<Item>();
    public List<Transform> displaypoints=new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        Stock();
    }

    // Update is called once per frame
    void Update()
    {
       
        //Check if enough time since restock has passed
        if (Time.time > restockTimer)
        {
            Restock();
           
            foreach(ShopPlot plot in FindObjectsOfType<ShopPlot>())
            {
                
            }
        }
    }
    void Unstock()
    {

        for (int i = displayedItems.Count; i > 0; i--)
        {
            displayedItems[0].gameObject.SetActive(false);
            availableItems.Add(displayedItems[0]);
            displayedItems.RemoveAt(0);
        }

    }

    void Stock()
    {
        for (int i = 0; i < displaypoints.Count; i++)
        {
            availableItems[0].gameObject.SetActive(true);
            availableItems[0].transform.position = displaypoints[i].position;
            displayedItems.Add(availableItems[0]);
            availableItems.RemoveAt(0);


        }

        foreach (ShopPlot plot in FindObjectsOfType<ShopPlot>())
        {
            plot.issoldout = false;
            plot.ItemDisplay.sprite = displayedItems[plot.plotid].Item_Display;
            plot.CostDisplay.text = "-" + displayedItems[plot.plotid].item_cost.ToString() + "GP";
            plot.SoldOut.gameObject.SetActive(false);
            plot.ispurchased = false;
        }
    }

    void Restock()
        //resets the restock timer at the end.
    {
        Unstock();
        Stock();

        //Reset restockTimer
        restockTimer = Time.time + timeToRestock;
    }

    

}
