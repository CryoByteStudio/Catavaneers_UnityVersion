using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
// Write by Will

public class SwitchCanvas : MonoBehaviour
{
    public GameObject TurnOffCanvas;
    public GameObject TurnOnCanvas;
    public GameObject FirstOhject;

    public void Switch()
    {
        TurnOffCanvas.SetActive(false);
        TurnOnCanvas.SetActive(true);
        GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(FirstOhject, null);
    }
}
