using System.Collections;
using System.Collections.Generic;

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.Audio;

public enum SoundClipsInts { Default, GoldPickUp, Attack, Hit, Death, Buying };

public class MusicManager : MonoBehaviour
{

    //The AudioSource to which we play any clips
    private AudioSource A_Source;

    //The audioclips which you should assign through inspector
    public AudioClip Clip_default_;
    public AudioClip Clip_GoldPickUp;
    public AudioClip Clip_Attack;
    public AudioClip Clip_Hit;
    public AudioClip Clip_Death;
    public AudioClip Clip_Buying;

    //Singleton accessor
    public static MusicManager Instance;

    void Awake()
    { Instance = this; }

    void Start()
    {
        //Add the audio source
        if (!GetComponent<AudioSource>())
        {
            A_Source = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            A_Source = GetComponent<AudioSource>();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Menu_Main")
        {
            Instance.PlaySoundTrack(SoundClipsInts.Default);
            Debug.Log("soundtrack default");
        }
    }

    public void PlaySoundTrack(SoundClipsInts TrackID)
    {

        //Stop any playing music
        A_Source.Stop();

        switch (TrackID)
        {
            case SoundClipsInts.GoldPickUp:
                A_Source.PlayOneShot(Clip_GoldPickUp);
                break;

            case SoundClipsInts.Attack:
                A_Source.PlayOneShot(Clip_Attack);
                break;

            case SoundClipsInts.Hit:
                A_Source.PlayOneShot(Clip_Hit);
                break;

            case SoundClipsInts.Death:
                A_Source.PlayOneShot(Clip_Death);
                break;

            case SoundClipsInts.Buying:
                A_Source.PlayOneShot(Clip_Buying);
                break;

            default:
                A_Source.PlayOneShot(Clip_default_);
                break;
        }

    }


}
