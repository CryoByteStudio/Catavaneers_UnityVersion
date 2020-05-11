using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveDefenceCameraController : MonoBehaviour
{
    //Set this list to include all of the players you want the camera to follow.
    public List<Transform> camtargets = new List<Transform>();
    public Vector3 zeropoint;
    public Camera cam;
    public float zMax=8f;
    public float zMin = -2f;
    public float xLimit=15f;
    public float ScaleFactorThreshold;
    public float camymax;
    public float camymin;
    public float xoffset;
    public float zoffset;
    // Start is called before the first frame update
    void Start()
    {

        cam = GetComponent<Camera>();
        zeropoint = new Vector3(cam.transform.position.x, cam.transform.position.y, cam.transform.position.z);
    }

    // Update is called once per frame

    private void Update()
    {
        RefreshCameraPos();
    }
    void RefreshCameraPos()
    {
        //Start at whatever was defined as the cameras 0 point
        float xpos=zeropoint.x+xoffset;
        float ypos = zeropoint.y;
        float zpos =zeropoint.z+zoffset;
        float highestx=zeropoint.x;
        float lowestx=zeropoint.x;
        float highestz=zeropoint.z;
        float lowestz=zeropoint.z;

       foreach(Transform target in camtargets)
        {
            //check for lowest/highest
            if (zeropoint.x + target.position.x > highestx)
            {
                highestx = zeropoint.x + target.position.x;
            }
            if (zeropoint.x + target.position.x < lowestx)
            {
                lowestx = zeropoint.x + target.position.x;
            }
            if (zeropoint.z + target.position.z > highestz)
            {
                highestz = zeropoint.z + target.position.z;
            }
            if (zeropoint.z + target.position.z < lowestz)
            {
                lowestz = zeropoint.z + target.position.z;
            }


            //Add the distances from zeropoint to the targets current pos
            xpos += target.position.x;
            zpos += target.position.z;

            
        }
       //check cam within x limits  pos left/right
        if (xpos > zeropoint.x+ xLimit+xoffset) 
        {
            Debug.Log("Camera above y buffer");
            xpos = xLimit+xoffset;
        }
        if( xpos < zeropoint.x- xLimit+xoffset)
        {
            Debug.Log("Camera below x buffer");
            xpos = -xLimit+xoffset;
        }
        //Check z limits   pos up/down
        if (zpos > zeropoint.z+ zMax+zoffset)
        {
            Debug.Log("Camera above z buffer");
            zpos = zeropoint.z+zMax;
        }
        if (zpos < zeropoint.z+ zMin+zoffset)
        {
            Debug.Log("Camera below z buffer");
            zpos = zeropoint.z+zMin+zoffset;
        }

   
        ypos = ypos* (highestx - lowestx)*ScaleFactorThreshold;

        // ypos = ypos * (highestz - lowestz);
        ypos /= (ypos / zeropoint.y + 1.5f);
        //check y limits   scale in/out
        if (ypos > camymax)
        {
            ypos = camymax;
        }
        if (ypos < camymin)
        {
            ypos = camymin;
        }
      


        //set cameras transform to the center of all objects.
        cam.transform.position = new Vector3(xpos + xoffset,ypos, zpos + zoffset) ;

    }
}
