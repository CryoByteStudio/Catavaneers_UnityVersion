using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicOption : MonoBehaviour
{
    public Toggle fullScreenTog, vsyncTog;

    // Start is called before the first frame update
    void Start()
    {
        //if(Screen.fullScreen == true)
        //{
        //    fullScreenTog.isOn = true;
        //}
        //else
        //{
        //    fullScreenTog.isOn = false;
        //}

        //if(QualitySettings.vSyncCount == 1)
        //{
        //    vsyncTog.isOn = true;
        //}
        //else
        //{
        //    vsyncTog.isOn = false;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyGraphics()
    {
        Screen.fullScreen = fullScreenTog.isOn;

        if(vsyncTog.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
    }
}
