using UnityEngine;
using ObjectPooling;
using UnityEngine.UI;
using System;
using System.Collections;
using AI;
using SpawnSystem;
using UnityEditor;
using ViTiet.Utils;
using Catavaneer;
using AI.States;

public enum CharacterClass { Player, Enemy, Caravan, Obj };
public enum DifficultyLevel { Normal = 5, IronCat = 10, Catapocalypse = 25, Catfight = 1};
[RequireComponent(typeof(AudioSource))]
public class HealthComp : MonoBehaviour
{
    public AudioSource A_Source;
    [SerializeField] private DifficultyLevel gameDifficulty = DifficultyLevel.Normal;
    public CharacterClass myClass;
    public int startHealth = 100;
    public GameManager gman;
    public bool caravan = false;
    public int damagethreshold;
    public int thresholdamount;

    public static event Action OnCaravanDestroyed;
    public SoundClipsInts soundCue = SoundClipsInts.Death;

    [SerializeField]
    private int currentHealth = 0;
    [SerializeField]
    private bool is_Regenerating = false;
    [SerializeField]
    private float dmg_percentage;
    [SerializeField]
    Transform playerSpawnPos;
    [SerializeField]
    SkinnedMeshRenderer playerMeshRenderer;

    GameObject hitParticle = null;

    private float nextDamageTime = 0;
    private float timeElapsed = 0;
    float percentageOfGoldToKeep = 100f;
    private bool isDead = false;
    private Rigidbody rb;
    private DropController dropController;

    private CharacterFader characterFader;

    public Slider health_slider = null;

    private static ObjectPooler objectPooler;
    Animator animator;

    [HideInInspector] public bool debug;
    [HideInInspector] public int damageTakenPerSecond;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        dropController = GetComponent<DropController>();
        objectPooler = FindObjectOfType<ObjectPooler>();
        animator = GetComponent<Animator>();
        characterFader = GetComponent<CharacterFader>();
        A_Source = GetComponent<AudioSource>();

        if (myClass == CharacterClass.Enemy)
        {
        }
        else if (myClass == CharacterClass.Caravan)
        {
        }
        else if (myClass == CharacterClass.Obj)
        {
        }

        if (health_slider)
        {
            health_slider.maxValue = startHealth;
        }

        currentHealth = startHealth;
        DisplayHealth();

        if (GetComponent<PlayerInventory>())
        {
            //set % of gold to lose based on difficulty
            switch (GameManager.Instance.DifficultyLevel)
            {
                case DifficultyLevel.Normal:
                    percentageOfGoldToKeep = 0.75f;
                    break;
                case DifficultyLevel.IronCat:
                    percentageOfGoldToKeep = 0.50f;
                    break;
                case DifficultyLevel.Catapocalypse:
                    percentageOfGoldToKeep = 0.25f;
                    break;
                case DifficultyLevel.Catfight:
                    EditorHelper.NotSupportedException("DifficultyLevel.Catfight");
                    break;
                default:
                    EditorHelper.NotSupportedException("default");
                    break;
            }
        }
        else
        {
            //set % of gold to lose based on difficulty
            switch (GameManager.Instance.DifficultyLevel)
            {
                case DifficultyLevel.Normal:
                    //EditorHelper.NotSupportedException("DifficultyLevel.Normal");
                    break;
                case DifficultyLevel.IronCat:
                    currentHealth = Mathf.RoundToInt(currentHealth * 1.3f);
                    startHealth = Mathf.RoundToInt(currentHealth * 1.3f);
                    health_slider.maxValue = currentHealth;
                    health_slider.value = currentHealth;
                    break;
                case DifficultyLevel.Catapocalypse:
                    currentHealth = Mathf.RoundToInt(currentHealth * 1.8f);
                    startHealth = Mathf.RoundToInt(currentHealth * 1.8f);
                    health_slider.maxValue = currentHealth;
                    health_slider.value = currentHealth;
                    break;
                case DifficultyLevel.Catfight:
                    EditorHelper.NotSupportedException("DifficultyLevel.Catfight");
                    break;
                default:
                    EditorHelper.NotSupportedException("default");
                    break;
            }
        }

    }

    private void Update()
    {
        if (debug)
            TestTakeDamage();

        if (SpawnManager.EnemiesAlive == 0)
        {
            SetIsWaveComplete(true);
        }
        else
        {
            SetIsWaveComplete(false);
        }

        timeElapsed += Time.deltaTime;

        if (myClass == CharacterClass.Caravan && is_Regenerating)
        {
            dmg_percentage = currentHealth % (startHealth / (int)GameManager.Instance.DifficultyLevel);

            if (dmg_percentage == 0)
            {
                is_Regenerating = false;
            }
            else { AddHealth(1); }
        }

    }

    private void Reset()
    {
        isDead = false;
        currentHealth = startHealth;
        health_slider.value = currentHealth;
        GetComponent<CapsuleCollider>().enabled = true;
        playerMeshRenderer.enabled = true;
    }

    /// <summary>
    /// Subtracting health by a preset amount every second for debugging purpose
    /// </summary>
    private void TestTakeDamage()
    {
        if (timeElapsed > nextDamageTime)
        {
            nextDamageTime = timeElapsed + 1f;
            TakeDamage(damageTakenPerSecond);
        }
    }

    /// <summary>
    /// Subtract health by some amount
    /// </summary>
    /// <param name="amount"> The amount that will be subtracted from health </param>
    public void TakeDamage(int amount)
    {
        if (!isDead)
        {
            currentHealth -= amount;
            currentHealth = Mathf.Max(0, currentHealth);
            DisplayHealth();
            if(myClass == CharacterClass.Player)
            {
                A_Source.clip = MusicManager.Instance.Clip_Hit;
                A_Source.volume = MusicManager.Instance.sfxVolume - 0.2f;
                A_Source.Play();
                //MusicManager.Instance.PlaySoundTrack(SoundClipsInts.Hit);
                Debug.Log("Hit");
            }else if(myClass == CharacterClass.Enemy)
            {
                A_Source.clip = MusicManager.Instance.Clip_Attack;
                A_Source.volume = MusicManager.Instance.sfxVolume - 0.2f;
                A_Source.Play();
                // MusicManager.Instance.PlaySoundTrack(SoundClipsInts.Attack);
                Debug.Log("Attack");
            }
            if (currentHealth <= 0)
            {
                Dead();
            }
        }
    }

    /// <summary>
    /// Subtract health by some amount and knock back base on the damage dealer position
    /// </summary>
    /// <param name="damageDealer"> The transform of the damage dealer </param>
    /// <param name="amount"> The amount that will be subtracted from health </param>
    /// <param name="weaponForce"> The amount of knockback_force from the weapon </param>"
    public void TakeDamage(Transform damageDealer, int amount, float weaponForce)
    {
        if (!isDead)
        {
            currentHealth -= amount;
            currentHealth = Mathf.Max(0, currentHealth);
            DisplayHealth();
            if (myClass == CharacterClass.Player)
            {
                MusicManager.Instance.PlaySoundTrack(SoundClipsInts.Hit);
            }
            else if (myClass == CharacterClass.Enemy)
            {
                MusicManager.Instance.PlaySoundTrack(SoundClipsInts.Attack);
            }
            KnockBack((damageDealer.position - transform.position) * 2f * weaponForce);

            if (currentHealth == 0)
            {
                Dead();
            }
        }
    }

    /// <summary>
    /// Apply knock back force
    /// </summary>
    /// <param name="force"> The force that will be applied on the rigidbody </param>
    private void KnockBack(Vector3 force)
    {
        rb.isKinematic = false;
        rb.AddForce(force);
        rb.isKinematic = true;
    }

    /// <summary>
    /// Do stuff when dead
    /// </summary>
    private void Dead()
    {
        isDead = true;

        switch (myClass)
        {
            case CharacterClass.Player:
                Debug.Log("Player Dead");

                if (GameManager.Instance.DifficultyLevel == DifficultyLevel.Catfight)
                {
                    if (GetComponent<PlayerInventory>() == FindObjectOfType<Goldbag>().holdersInventory)
                    {
                        FindObjectOfType<Goldbag>().DropBag();
                    }
                }

                if (animator != null)
                {
                    DeathBehaviour();
                }

                break;
            case CharacterClass.Caravan:
                if (OnCaravanDestroyed != null)
                    OnCaravanDestroyed.Invoke();
                Debug.Log("Caravan Dead");
                break;
            case CharacterClass.Obj:
                dropController.DropItem();
                gameObject.SetActive(false);
                break;
            case CharacterClass.Enemy:
                //if (GetComponent<Animator>() != null)
                //{
                //    GetComponent<Animator>().SetTrigger("Die");
                //}
                dropController.DropItem();
                //ObjectPooler.SetInactive(this.gameObject);
                SpawnManager.EnemiesAlive--;
                break;
        }
    }

    private void DeathBehaviour()
    {
        animator.SetTrigger("Die");
        StartCoroutine(RespawnRoutine());
    }

    /// <summary>
    /// Handles player respawn when dead
    /// </summary>
    private IEnumerator RespawnRoutine()
    {
        int deathAnimationDuration = 3;
        int fadeOutDuration = 5;

        yield return new WaitForSeconds(deathAnimationDuration);
        if (characterFader)
            characterFader.FadeOut(fadeOutDuration);

        yield return new WaitForSeconds(fadeOutDuration);
        playerMeshRenderer.enabled = false;

        //make sure its a player first
        if (myClass == CharacterClass.Player)
            GetComponent<PlayerInventory>().RemoveGoldFromInventory(percentageOfGoldToKeep);

        switch (GameManager.Instance.DifficultyLevel)
        {
            case DifficultyLevel.Normal:
                yield return new WaitForSeconds(4);
                break;
            case DifficultyLevel.IronCat:
                yield return new WaitForSeconds(8);
                break;
            case DifficultyLevel.Catapocalypse:
                yield return new WaitForSeconds(12);
                break;
        }

        transform.position = playerSpawnPos.position;
        characterFader.ResetFade();
        Reset();
        Controller.AddToTargetList(this);
        animator.SetTrigger("Spawn");
        //TODO Play Particle FX
    }

    private IEnumerator Spawn(float timer)
    {
        yield return new WaitForSeconds(timer);
    }

    /// <summary>
    /// Add to health by some amount
    /// </summary>
    /// <param name="amount"> The amount that will be added to health </param>
    public void AddHealth(int amount)
    {
        if (!isDead)
        {
            currentHealth += amount;
            currentHealth = Mathf.Min(currentHealth, startHealth);
            DisplayHealth();
            if(myClass == CharacterClass.Player && currentHealth < startHealth)
            {
                MusicManager.Instance.PlaySoundTrack(SoundClipsInts.Bandage);
            }
        }
    }

    /// <summary>
    /// returns currentHealth amount
    /// </summary>
    public int GetCurHealth()
    {
        return currentHealth;
    }

    /// <summary>
    /// returns StartHealth amount
    /// </summary>
    public int GetStartHealth()
    {
        return startHealth;
    }

    /// <summary>
    /// Returns if character is dead
    /// </summary>
    public bool IsDead()
    {
        return isDead;
    }

    public void SetIsDead(bool isDead)
    {
        this.isDead = isDead;
    }

    /// <summary>
    /// sets regeneration to true. to be used for the caravan
    /// </summary>
    public void SetIsWaveComplete(bool is_wave_complete)
    {
        is_Regenerating = is_wave_complete;
    }

    /// <summary>
    /// Displays health on the health slider
    /// </summary>
    private void DisplayHealth()
    {

        if (hitParticle)
        {
            GameObject temp = Instantiate(hitParticle.gameObject);
            temp.transform.parent = null;
            temp.transform.position = this.transform.position;

            temp.GetComponent<ParticleSystem>().Play();
            Destroy(temp.gameObject, 1f);
        }

        if (health_slider)
            health_slider.value = currentHealth;

        if (caravan)
        {
            if (currentHealth <= 0)
            {
                GetComponent<CaravanDamage>().TriggerFinalDamageStageParticle();
            }
            else if (currentHealth <= damagethreshold) {
                damagethreshold -= thresholdamount;
            GetComponent<CaravanDamage>().TriggerDamageStageParticles();
            }
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(HealthComp))]
public class MyScriptEditor : Editor
{
    override public void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        HealthComp healthComp = target as HealthComp;

        healthComp.debug = GUILayout.Toggle(healthComp.debug, "Debug");

        if (healthComp.debug)
            healthComp.damageTakenPerSecond = EditorGUILayout.IntField("Damage Taken Per Second", healthComp.damageTakenPerSecond);

    }
}
#endif
