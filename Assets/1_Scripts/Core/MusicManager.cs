using System.Collections;
using System.Collections.Generic;

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.Audio;
using SpawnSystem;
using Catavaneer.MenuSystem;
using Catavaneer.Singleton;
using Catavaneer.LevelManagement;

public enum SoundClipsInts { Default, GoldPickUp, Attack, Hit, Death, Buying, Bandage, TrapTrigger };

[RequireComponent(typeof(AudioSource))]
public class MusicManager : SingletonEntity<MusicManager>
{
    [FMODUnity.EventRef]
    public string caravanStateEvent = "";
    public FMOD.Studio.EventInstance caravanState;
    [SerializeField] HealthComp caravanHealth;
    float curCaravanIntensity = 0.0f;
    float curEnemieIntensity = 0.0f;
    [FMODUnity.EventRef]
    public string menuStateEvent = "";
<<<<<<< HEAD
    public FMOD.Studio.EventInstance menuState;
    public float sfxVolume;
    private bool doneOnce = false;
=======
    FMOD.Studio.EventInstance menuState;
    bool isPLayingEvent = false;
<<<<<<< HEAD
>>>>>>> a3e16226b2f3ac81c8361cf9fa27d6f8ae1b5855
=======
>>>>>>> a3e16226b2f3ac81c8361cf9fa27d6f8ae1b5855

    //The AudioSource to which we play any clips
    private AudioSource A_Source;
    public bool isMuted;

    //The audioclips which you should assign through inspector
    public AudioClip Clip_default;
    public AudioClip Clip_GoldPickUp;
    public AudioClip Clip_Attack;
    public AudioClip Clip_Hit;
    public AudioClip Clip_Death;
    public AudioClip Clip_Buying;
    public AudioClip Clip_Bandage;
    public AudioClip Clip_TrapTrigger;

    //Singleton accessor
    //public static MusicManager Instance;

<<<<<<< HEAD
    //void  Awake()
    //{
    //    Instance = this;
    //}

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        //LevelLoader.OnSceneLoaded += SceneLoadedHandler;
        base.OnEnable();
    }

    protected override void SceneLoadedHandler(Scene scene, LoadSceneMode mode)
    {
        PlayMusic(scene);
        //LevelLoader.ResetLoadingParams();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        //LevelLoader.OnSceneLoaded -= SceneLoadedHandler;
    }

    private void PlayMusic(Scene scene)
    {
        if(scene.name == "Menu_Main")
        {
            menuState.start();
        }
        else if (!caravanHealth)
        {
            HealthComp[] healthComps = FindObjectsOfType<HealthComp>();
            if (healthComps.Length <= 0 || healthComps == null) return;
            for (int i = 0; i < healthComps.Length; i++)
            {
                if (healthComps[i].myClass == CharacterClass.Caravan)
                {
                    caravanHealth = healthComps[i];
                    break;
                }
            }
            caravanState.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            caravanState.start();
        }

=======
    void Awake()
    { 
        // Check if 'GameManager' instance exists
        if (Instance)
            // 'GameManager' already exists, delete copy
            Destroy(gameObject);
        else
        {
            // 'GameManager' does not exist so assign a reference to it
            Instance = this;
        }
<<<<<<< HEAD
>>>>>>> a3e16226b2f3ac81c8361cf9fa27d6f8ae1b5855
=======
>>>>>>> a3e16226b2f3ac81c8361cf9fa27d6f8ae1b5855
    }

    void Start()
    {
        caravanState = FMODUnity.RuntimeManager.CreateInstance(caravanStateEvent);
        
        menuState = FMODUnity.RuntimeManager.CreateInstance(menuStateEvent);
        menuState.start();
        isPLayingEvent = true;
<<<<<<< HEAD

        int MuteInt = PlayerPrefs.GetInt("Mutetogglestate");

        isMuted = MuteInt == 0 ? false : true;

        caravanState.setPaused(MuteInt == 0 ? false : true);
        menuState.setPaused(MuteInt == 0 ? false : true);
        sfxVolume = MuteInt == 0 ? PlayerPrefs.GetFloat("SFXVolume") : 0;

        caravanState.setVolume(PlayerPrefs.GetFloat("MVolume"));
        menuState.setVolume(PlayerPrefs.GetFloat("MVolume"));

        //Add the audio source
<<<<<<< HEAD
        A_Source = gameObject.GetComponent<AudioSource>();
        if (!caravanHealth)
        {
            HealthComp[] healthComps = FindObjectsOfType<HealthComp>();
            if (healthComps.Length <= 0 || healthComps == null) return;
            for (int i = 0; i < healthComps.Length; i++)
            {
                if (healthComps[i].myClass == CharacterClass.Caravan)
                {
                    caravanHealth = healthComps[i];
                    break;
                }
            }
        }
=======
        A_Source = gameObject.AddComponent<AudioSource>();

>>>>>>> a3e16226b2f3ac81c8361cf9fa27d6f8ae1b5855
=======

        //Add the audio source
        A_Source = gameObject.AddComponent<AudioSource>();

>>>>>>> a3e16226b2f3ac81c8361cf9fa27d6f8ae1b5855
    }


    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD
<<<<<<< HEAD
        
        if (SceneManager.GetActiveScene().name != "Menu_Main" && SceneManager.GetActiveScene().name != "Menu_CharacterSelect" && SceneManager.GetActiveScene().name != "SplashScreen")
=======
=======
>>>>>>> a3e16226b2f3ac81c8361cf9fa27d6f8ae1b5855
        if ((SceneManager.GetActiveScene().name == "Menu_Main" || SceneManager.GetActiveScene().name == "Menu_CharacterSelect" || SceneManager.GetActiveScene().name == "SplashScreen") && !isPLayingEvent)
        {
            menuState.start();
            isPLayingEvent = true;
        }
        else if (SceneManager.GetActiveScene().name != "Menu_Main" && SceneManager.GetActiveScene().name != "Menu_CharacterSelect" && SceneManager.GetActiveScene().name != "SplashScreen")
<<<<<<< HEAD
>>>>>>> a3e16226b2f3ac81c8361cf9fa27d6f8ae1b5855
=======
>>>>>>> a3e16226b2f3ac81c8361cf9fa27d6f8ae1b5855
        {
            menuState.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            isPLayingEvent = false;
        }
<<<<<<< HEAD
<<<<<<< HEAD
        if (LevelLoader.IsGameLevel())
        {
            
            if(caravanHealth != null)
=======
=======
>>>>>>> a3e16226b2f3ac81c8361cf9fa27d6f8ae1b5855
        if (SceneManager.GetActiveScene().name == "Encounter_01" || SceneManager.GetActiveScene().name == "Encounter_02")
        {
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
                caravanState.start();
            }
            if (caravanHealth.GetCurHealth() <= caravanHealth.GetStartHealth() / 4)
            {
                caravanState.setParameterByName("Caravan Health", Mathf.Lerp(curCaravanIntensity, 3f, 1.0f));
            }
            else if (caravanHealth.GetCurHealth() <= caravanHealth.GetStartHealth() / 2)
            {
                caravanState.setParameterByName("Caravan Health", Mathf.Lerp(curCaravanIntensity, 1.5f, 1.0f));
            }
            else
            {
                caravanState.setParameterByName("Caravan Health", 0.0f);
            }
            if (SpawnManager.EnemiesAlive < 10)
            {
                caravanState.setParameterByName("Intensity", Mathf.Lerp(curEnemieIntensity, 0f, 1.0f));
            }
            else if (SpawnManager.EnemiesAlive >= 10 && caravanHealth.GetCurHealth() > caravanHealth.GetStartHealth() / 4)
            {
                caravanState.setParameterByName("Intensity", Mathf.Lerp(curEnemieIntensity, 1.0f, 1.0f));
            }
            else if (SpawnManager.EnemiesAlive >= 10 && caravanHealth.GetCurHealth() <= caravanHealth.GetStartHealth() / 4)
>>>>>>> a3e16226b2f3ac81c8361cf9fa27d6f8ae1b5855
            {
                //if(!doneOnce)
                //{
                //    caravanState.start();
                //    doneOnce = true;
                //}

                if (caravanHealth.GetCurHealth() <= caravanHealth.GetStartHealth() / 4)
                {
                    caravanState.setParameterByName("Caravan Health", Mathf.Lerp(curCaravanIntensity, 3f, 1.0f));
                }
                else if (caravanHealth.GetCurHealth() <= caravanHealth.GetStartHealth() / 2)
                {
                    caravanState.setParameterByName("Caravan Health", Mathf.Lerp(curCaravanIntensity, 1.5f, 1.0f));
                }
                if (SpawnManager.EnemiesAlive < 10)
                {
                    caravanState.setParameterByName("Intensity", Mathf.Lerp(curEnemieIntensity, 0f, 1.0f));
                }
                else if (SpawnManager.EnemiesAlive >= 10 && caravanHealth.GetCurHealth() > caravanHealth.GetStartHealth() / 4)
                {
                    caravanState.setParameterByName("Intensity", Mathf.Lerp(curEnemieIntensity, 1.0f, 1.0f));
                }
                else if (SpawnManager.EnemiesAlive >= 10 && caravanHealth.GetCurHealth() <= caravanHealth.GetStartHealth() / 4)
                {
                    caravanState.setParameterByName("Intensity", Mathf.Lerp(curEnemieIntensity, 2.0f, 1.0f));
                }
            }
<<<<<<< HEAD

=======
            if(caravanHealth.GetCurHealth() <= 0)
            {
                caravanState.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
<<<<<<< HEAD
>>>>>>> a3e16226b2f3ac81c8361cf9fa27d6f8ae1b5855
=======
            if(caravanHealth.GetCurHealth() <= 0)
            {
                caravanState.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
>>>>>>> a3e16226b2f3ac81c8361cf9fa27d6f8ae1b5855
        }
        //else if (SceneManager.GetActiveScene().name != "Encounter_01" && SceneManager.GetActiveScene().name != "Encounter_02")
        //{
        //    caravanState.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        //    isPLayingEvent = false;
        //}
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
                A_Source.PlayOneShot(Clip_GoldPickUp, sfxVolume);
                break;

            case SoundClipsInts.Attack:
<<<<<<< HEAD
<<<<<<< HEAD
                A_Source.PlayOneShot(Clip_Attack, sfxVolume);
                break;

            case SoundClipsInts.Hit:
                A_Source.PlayOneShot(Clip_Hit, sfxVolume);
=======
=======
>>>>>>> a3e16226b2f3ac81c8361cf9fa27d6f8ae1b5855
                A_Source.PlayOneShot(Clip_Attack,0.8f);
                break;

            case SoundClipsInts.Hit:
                A_Source.PlayOneShot(Clip_Hit,0.8f);
<<<<<<< HEAD
>>>>>>> a3e16226b2f3ac81c8361cf9fa27d6f8ae1b5855
=======
>>>>>>> a3e16226b2f3ac81c8361cf9fa27d6f8ae1b5855
                break;

            case SoundClipsInts.Death:
                A_Source.PlayOneShot(Clip_Death, sfxVolume);
                break;

            case SoundClipsInts.Buying:
                A_Source.PlayOneShot(Clip_Buying, sfxVolume);
                break;

            case SoundClipsInts.Bandage:
                A_Source.PlayOneShot(Clip_Bandage, 1);
                break;

            case SoundClipsInts.TrapTrigger:
                A_Source.PlayOneShot(Clip_TrapTrigger, 1);
                break;

            case SoundClipsInts.Bandage:
                A_Source.PlayOneShot(Clip_Bandage, 1);
                break;

            case SoundClipsInts.TrapTrigger:
                A_Source.PlayOneShot(Clip_TrapTrigger, 1);
                break;

            default:
                A_Source.PlayOneShot(Clip_default, sfxVolume);
                break;
        }

    }


}
