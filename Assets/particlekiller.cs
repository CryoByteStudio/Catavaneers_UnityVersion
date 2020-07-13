using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particlekiller : MonoBehaviour
{
    public ParticleSystem part;

    public float timetodestroy=0;
    bool hasdestroytime = false;
    private void Start()
    {
        part = GetComponent<ParticleSystem>();
        
    }

    private void Update()
    {
        if (part.isPlaying)
        {
            //Debug.Log("playing");
            hasdestroytime = false;
        }
        if (!part.isPlaying&&!hasdestroytime)
        {
            //Debug.Log("not plyaing");
            timetodestroy = Time.time + 0.5f;
            hasdestroytime = true;
        }
        if (Time.time >= timetodestroy && hasdestroytime)
        {
            //Debug.Log("destroy");
            Destroy(this.gameObject);
        }
    }
}
