using System.Collections;
using System.Collections.Generic;

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.Audio;
using SpawnSystem;

public enum SoundClipsInts { Default, GoldPickUp, Attack, Hit, Death, Buying };

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string caravanStateEvent = "";
    FMOD.Studio.EventInstance caravanState;
    [SerializeField] HealthComp caravanHealth;
    float curCaravanIntensity = 0.0f;
    float curEnemieIntensity = 0.0f;


    //The AudioSource to which we play any clips
    private AudioSource A_Source;

    //The audioclips which you should assign through inspector
    public AudioClip Clip_default;
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
        caravanState = FMODUnity.RuntimeManager.CreateInstance(caravanStateEvent);
        caravanState.start();


        //Add the audio source
        A_Source = gameObject.AddComponent<AudioSource>();
        if (!caravanHealth)
        {
            HealthComp[] healthComps = FindObjectsOfType<HealthComp>();
            for (int i = 0; i < healthComps.Length; i++)
            {
                if (healthComps[i].myClass == CharacterClass.Caravan)
                {
                    caravanHealth = healthComps[i];
                    break;
                }
            }
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
        if (SceneManager.GetActiveScene().name == "Encounter_01")
        {
            if (caravanHealth.GetCurHealth() <= caravanHealth.GetStartHealth() / 4)
            {
                caravanState.setParameterByName("Caravan Health", Mathf.Lerp(curCaravanIntensity, 3f, 1.0f), true);
            }
            else if (caravanHealth.GetCurHealth() <= caravanHealth.GetStartHealth() / 2)
            {
                caravanState.setParameterByName("Caravan Health", Mathf.Lerp(curCaravanIntensity, 1.5f, 1.0f), true);
            }
            if (SpawnManager.EnemiesAlive < 10)
            {
                caravanState.setParameterByName("Intensity", Mathf.Lerp(curEnemieIntensity, 0f, 1.0f));
            }
            else if (SpawnManager.EnemiesAlive >= 10 && caravanHealth.GetCurHealth() > caravanHealth.GetStartHealth() / 4)
            {
                caravanState.setParameterByName("Intensity", Mathf.Lerp(curEnemieIntensity, 1.0f, 1.0f), true);
            }
            else if (SpawnManager.EnemiesAlive >= 10 && caravanHealth.GetCurHealth() <= caravanHealth.GetStartHealth() / 4)
            {
                caravanState.setParameterByName("Intensity", Mathf.Lerp(curEnemieIntensity, 2.0f, 1.0f), true);
            }
        }
        else
        {
            caravanState.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }

    public void PlaySoundTrack(SoundClipsInts TrackID)
    {

        //Stop any playing music
        if (A_Source == null) return;

        A_Source.Stop();

        switch (TrackID)
        {
            case SoundClipsInts.GoldPickUp:   
                Debug.Log("A SOURCE!!!!!!!!!!!!!!!!!!!! " + A_Source);
                Debug.Log("CLIP GOLD!!!!!!!!!!!!!!!!!!! " + Clip_GoldPickUp);
                A_Source.PlayOneShot(Clip_GoldPickUp, 1);

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
                A_Source.PlayOneShot(Clip_Buying,1);
                break;

            default:
                A_Source.PlayOneShot(Clip_default);
                break;
        }

    }


}
