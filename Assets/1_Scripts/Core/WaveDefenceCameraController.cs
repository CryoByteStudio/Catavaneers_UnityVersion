using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveDefenceCameraController : MonoBehaviour
{
    //Set this list to include all of the players you want the camera to follow.
    public List<Transform> camtargets = new List<Transform>();
    public Vector3 zeropoint;
    public Camera cam;
   
    public float camxmax=12f;
    public float camxmin=-12f;
    public float camzmax=12f;
    public float camzmin=-12f;
    public float camymax = 25f;
    public float camymin = 15f;
    
    public float zoffset;
    public float zoom=1f;
    public float currentxlow = 0;
    public float currentxhigh= 0;
    public float currentzlow = 0;
    public float currentzhigh = 0;
    public float zoomxfactor;
    public float zoomzfactor;

    public float minxdist;
    public float minzdist;


    public float distx=0f;
    public float distz = 0f;

    // Start is called before the first frame update
    void Start()
    {

        cam = GetComponent<Camera>();
        zeropoint = new Vector3(cam.transform.position.x, cam.transform.position.y, cam.transform.position.z+zoffset);
        camxmax = zeropoint.x + camxmax;
        camxmin = zeropoint.x + camxmin;
      
}

    

    private void Update()
    {
        RefreshCameraPos();
    }
    void RefreshCameraPos()
    {
        //Start at whatever was defined as the cameras 0 point
        float xpos=zeropoint.x;
        float ypos = zeropoint.y;
        float zpos =zeropoint.z;

        float highestx=camtargets[0].position.x;
        float highestz = camtargets[0].position.z;
        float lowestz = camtargets[0].position.z; 
        float lowestx = camtargets[0].position.x; 
      
       
       
       foreach(Transform target in camtargets)
        {

            //get absolute x distance from zeropoint

            float distfromzerox= Mathf.Abs(zeropoint.x - target.position.x);
            float distfromzeroz = Mathf.Abs(zeropoint.z - target.position.z);
            if (target.position.x > zeropoint.x)
            {
                //add to zeropoint
                xpos += distfromzerox;
            }
            else
            {
                //subtract from zeropoint
                xpos -= distfromzerox;
            }
            if (target.position.z > zeropoint.z)
            {
                //add to zeropoint
                zpos += distfromzeroz;
            }
            else
            {
                //subtract from zeropoint
               zpos -= distfromzeroz;
            }




            //calculate highests

            if (target.position.x < lowestx)
            {
                lowestx = distfromzerox;
            }
          
            if (target.position.x > highestx)
            {
                highestx = distfromzerox;
            }
            if (target.position.z < lowestz)
            {
                lowestz = distfromzeroz;
            }

            if (target.position.z > highestz)
            {
                highestz = distfromzeroz;
            }


        }
        currentxhigh = highestx;
        currentxlow = lowestx;
        currentzhigh = highestz;
        currentzlow = lowestz;
        distx = Mathf.Abs(currentxhigh - currentxlow);
        distz = Mathf.Abs(currentzhigh - currentzlow);

        if (distx < minxdist)
        {
            distx = minxdist;
        }
        if (distz < minzdist)
        {
            distz = minzdist;
        }

        zoom = (distx * zoomxfactor)+(distz*zoomzfactor);
     


        
            ypos = zoom;
        

        //clamp x pos
        if (xpos > camxmax)
        {
            xpos=camxmax;

        }else if (xpos < camxmin)
        {
            xpos = camxmin;
        }
        //clamp z pos
        if (zpos > camzmax)
        {
            zpos = camzmax;

        }
        else if (zpos < camzmin)
        {
            zpos = camzmin;
        }
        //clamp y pos
        if (ypos > camymax)
        {
            ypos = camymax;

        }
        else if (ypos < camymin)
        {
            ypos = camymin;
        }




        //set cameras transform to the center of all objects.
        cam.transform.position = new Vector3(xpos ,ypos, zpos ) ;

    }
}
