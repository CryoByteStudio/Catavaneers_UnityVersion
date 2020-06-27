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
    public CharacterClass myClass;
    public int startHealth = 100;
    public GameManager gman;
    public int damagethreshold;
    public int firstthreshold;
    public int thresholdamount;
    public int lasthitthreshold;

    public float healthscale2P=1.1f;
    public float healthscale3P=1.3f;
    public float healthscale4P=1.5f;
    public float healthscaleIroncat=1.3f;
    public float healthscaleCatpoc=1.6f;



    public static event Action<HealthComp> OnCaravanDestroyed;
    public event Action<HealthComp> OnEnemyHealthChanged;
    public event Action<HealthComp> OnEnemyDeath;
    public event Action<HealthComp> OnPlayerHealthChanged;
    public event Action<HealthComp> OnPlayerDeath;
    public event Action<HealthComp> OnCaravanHealthChanged;
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

    Animator animator;
    private float playerHealthScale;
    public Text healthuitext;

    [HideInInspector] public bool debug;
    [HideInInspector] public int damageTakenPerSecond;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        dropController = GetComponent<DropController>();
        animator = GetComponent<Animator>();
        characterFader = GetComponent<CharacterFader>();
        A_Source = GetComponent<AudioSource>();

        firstthreshold = damagethreshold;

        ScalePlayerHealth();
        SetGoldDropPercentage();
        ScaleEnemyHealth();

        currentHealth = startHealth;
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        switch (myClass)
        {
            case CharacterClass.Player:
                if (OnPlayerHealthChanged != null)
                    OnPlayerHealthChanged.Invoke(this);
                break;
            case CharacterClass.Enemy:
                if (OnEnemyHealthChanged != null)
                    OnEnemyHealthChanged.Invoke(this);
                break;
            case CharacterClass.Caravan:
                if (OnCaravanHealthChanged != null)
                    OnCaravanHealthChanged.Invoke(this);
                break;
            case CharacterClass.Obj:
            default:
                break;
        }
    }

    private void SetGoldDropPercentage()
    {
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
    }

    private void ScaleEnemyHealth()
    {
        if (myClass == CharacterClass.Enemy)
        {
            //set enemy health
            switch (GameManager.Instance.DifficultyLevel)
            {
                case DifficultyLevel.Normal:
                    break;
                case DifficultyLevel.IronCat:
                    startHealth = Mathf.RoundToInt(startHealth * healthscaleIroncat * playerHealthScale);
                    break;
                case DifficultyLevel.Catapocalypse:
                    startHealth = Mathf.RoundToInt(startHealth * healthscaleCatpoc * playerHealthScale);
                    break;
                case DifficultyLevel.Catfight:
                    break;
                default:
                    break;
            }
        }
    }

    private void ScalePlayerHealth()
    {
        if (CharacterManager.Instance)
        {
            switch (CharacterManager.Instance.playerCount)
            {
                case 1:
                    playerHealthScale = 1f;
                    break;
                case 2:
                    playerHealthScale = healthscale2P;
                    break;
                case 3:
                    playerHealthScale = healthscale3P;
                    break;

                case 4:
                    playerHealthScale = healthscale4P;
                    break;
                default:
                    playerHealthScale = 1f;
                    Debug.LogWarning("problem finding character manager on health comp, defaulting playerhealthscale to 1");
                    break;
            }
        }
    }

    private void FixedUpdate()
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

        DisplayHealth();

        if (myClass == CharacterClass.Caravan && is_Regenerating)
        { 
            if (currentHealth == startHealth)
            {
                is_Regenerating = false;
                GetComponent<CaravanDamage>().ResetDamage();
            }

            AddHealth(3);

            if (currentHealth > damagethreshold + thresholdamount)
            {
                damagethreshold += thresholdamount;
                if (damagethreshold > firstthreshold)
                {
                    damagethreshold = firstthreshold;
                    GetComponent<CaravanDamage>().ResetDamage();
                }
                else
                {
                    GetComponent<CaravanDamage>().ReverseDamageStageParticles();
                }
            }
        }
    }
  

    private void Reset()
    {
        isDead = false;
        currentHealth = startHealth;

        if (OnPlayerHealthChanged != null)
            OnPlayerHealthChanged.Invoke(this);

        GetComponent<CapsuleCollider>().enabled = true;

        if (myClass == CharacterClass.Player)
            playerMeshRenderer.enabled = true;
    }

    public void ResetHealthComp()
    {
        Reset();
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

            OnTakeDamageBehaviour();

            if (currentHealth <= 0)
                Dead();
        }
    }

    public void TakeDamage(Fighter damageDealer, int amount)
    {
        if (!isDead)
        {
            currentHealth -= amount;
            currentHealth = Mathf.Max(0, currentHealth);

            OnTakeDamageBehaviour();

            if (currentHealth <= 0)
                Dead(damageDealer);
        }
    }

    private void OnTakeDamageBehaviour()
    {
        switch (myClass)
        {
            case CharacterClass.Player:
                if (OnPlayerHealthChanged != null)
                    OnPlayerHealthChanged.Invoke(this);

                if (currentHealth <= damagethreshold && currentHealth > 0)
                {
                    lasthitthreshold = damagethreshold;
                    damagethreshold -= thresholdamount;
                    GetComponent<CaravanDamage>().TriggerDamageStageParticles();
                    
                }

                A_Source.clip = MusicManager.Instance.Clip_Hit;
                A_Source.volume = MusicManager.Instance.sfxVolume - 0.2f;
                A_Source.Play();
                break;
            case CharacterClass.Enemy:
                if (OnEnemyHealthChanged != null)
                    OnEnemyHealthChanged.Invoke(this);

                A_Source.clip = MusicManager.Instance.Clip_Attack;
                A_Source.volume = MusicManager.Instance.sfxVolume - 0.2f;
                A_Source.Play();
                break;
            case CharacterClass.Caravan:
                if (OnCaravanHealthChanged != null)
                    OnCaravanHealthChanged.Invoke(this);
                break;
            case CharacterClass.Obj:
            default:
                break;
        }
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
                PlayerDeathBehaviour();
                break;
            case CharacterClass.Caravan:
                CaravanDestroyedBehaviour();
                break;
            case CharacterClass.Enemy:
                EnemyDeathBehaviour();

                if (OnEnemyDeath != null)
                    OnEnemyDeath.Invoke(this);
                break;
            case CharacterClass.Obj:
            default:
                break;
        }
    }

    private void Dead(Fighter damageDealer)
    {
        isDead = true;

        switch (myClass)
        {
            case CharacterClass.Player:
                PlayerDeathBehaviour();
                break;
            case CharacterClass.Caravan:
                CaravanDestroyedBehaviour();
                break;
            case CharacterClass.Enemy:
                EnemyDeathBehaviour();
                damageDealer.AddKill();
                break;
            case CharacterClass.Obj:
            default:
                break;
        }
    }

    private void EnemyDeathBehaviour()
    {
        dropController.DropItem();

        SpawnManager.EnemiesAlive--;

        if (SpawnManager.EnemiesAlive <= 0)
        {
            FindObjectOfType<SpawnManager>().SetNextWaveTime();
        }
    }

    private void CaravanDestroyedBehaviour()
    {
        GetComponent<CaravanDamage>().TriggerFinalDamageStageParticle();

        if (OnCaravanDestroyed != null)
            OnCaravanDestroyed.Invoke(this);
    }

    private void PlayerDeathBehaviour()
    {
        if (GameManager.Instance.DifficultyLevel == DifficultyLevel.Catfight)
        {
            if (GetComponent<PlayerInventory>() == FindObjectOfType<Goldbag>().holdersInventory)
            {
                FindObjectOfType<Goldbag>().DropBag();
            }
        }

        if (animator != null)
        {
            animator.SetTrigger("Die");
            StartCoroutine(RespawnRoutine());
        }
    }

    /// <summary>
    /// Handles player respawn when dead
    /// </summary>
    private IEnumerator RespawnRoutine()
    {
        int deathAnimationDuration = 1;
        int fadeOutDuration = 2;

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
                yield return new WaitForSeconds(2);
                break;
            case DifficultyLevel.IronCat:
                yield return new WaitForSeconds(3);
                break;
            case DifficultyLevel.Catapocalypse:
                yield return new WaitForSeconds(4);
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

            switch (myClass)
            {
                case CharacterClass.Player:
                    if (OnPlayerHealthChanged != null)
                        OnPlayerHealthChanged.Invoke(this);
                    if (currentHealth < startHealth)
                        MusicManager.Instance.PlaySoundTrack(SoundClipsInts.Bandage);
                    break;
                case CharacterClass.Enemy:
                    if (OnEnemyHealthChanged != null)
                        OnEnemyHealthChanged.Invoke(this);
                    break;
                case CharacterClass.Caravan:
                    if (OnCaravanHealthChanged != null)
                        OnCaravanHealthChanged.Invoke(this);
                    break;
                case CharacterClass.Obj:
                default:
                    break;
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

        if (myClass == CharacterClass.Caravan)
        {
            if (currentHealth <= 0)
            {
                GetComponent<CaravanDamage>().TriggerFinalDamageStageParticle();
            }
            else if (currentHealth <= damagethreshold)
            {
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
