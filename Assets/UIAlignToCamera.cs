using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAlignToCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(2*transform.position-Camera.main.transform.position);
       // 2 * transform.position - stareat.position
    }
}
