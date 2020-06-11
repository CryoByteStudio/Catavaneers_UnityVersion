using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeTransformer : MonoBehaviour
{
    // Start is called before the first frame update

    float scalemax = 1.4f;
    float scalemin = 0.6f;
    float rotationmax = 360;
    float rotationmin = 0;
    void Start()
    {
        float randomnum = Random.Range(scalemin, scalemax);
        gameObject.isStatic = false;
        transform.localScale = new Vector3(randomnum,randomnum,randomnum);
        transform.Rotate (new Vector3(transform.rotation.x, Random.Range(rotationmin, rotationmax), transform.rotation.z));
        gameObject.isStatic = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
