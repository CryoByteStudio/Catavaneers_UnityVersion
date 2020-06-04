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

public enum CharacterClass { Player, Enemy, Caravan, Obj };
public enum DifficultyLevel { Normal, IronCat, Catapocalypse, Catfight};



public class HealthComp : MonoBehaviour
{
    [SerializeField] private DifficultyLevel gameDifficulty = DifficultyLevel.Normal;
    public CharacterClass myClass;
    public int startHealth = 100;
    public GameManager gman;
    public bool caravan = false;
    public int damagethreshold;
    public int thresholdamount;

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

        //if (FindObjectOfType<GameDifficultyManager>())
        //{
        //    gameDifficulty = FindObjectOfType<GameDifficultyManager>().dif;
        //}

        if (myClass == CharacterClass.Enemy)
        {
            //dropController = GetComponent<DropController>();
            //objectPooler = FindObjectOfType<ObjectPooler>();
        }
        else if (myClass == CharacterClass.Caravan)
        {

        }
        else if (myClass == CharacterClass.Obj)
        {
            //dropController = GetComponent<DropController>();
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
            switch (GameManager.DifficultyLevel)
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
            //if (gameDifficulty == DifficultyLevel.Normal)
            //{
            //    percentageOfGoldToKeep = 75f;
            //}
            //else if (gameDifficulty == DifficultyLevel.IronCat)
            //{
            //    percentageOfGoldToKeep = 50f;
            //}
            //else if (gameDifficulty == DifficultyLevel.Catapocalypse)
            //{
            //    percentageOfGoldToKeep = 25f;
            //}
        }
        else
        {
            //set % of gold to lose based on difficulty
            switch (GameManager.DifficultyLevel)
            {
                case DifficultyLevel.Normal:
                    EditorHelper.NotSupportedException("DifficultyLevel.Normal");
                    break;
                case DifficultyLevel.IronCat:
                    currentHealth *= 2;
                    startHealth *= 2;
                    health_slider.maxValue = currentHealth;
                    health_slider.value = currentHealth;
                    break;
                case DifficultyLevel.Catapocalypse:
                    currentHealth *= 3;
                    startHealth *= 3;
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
            //if (gameDifficulty == DifficultyLevel.Normal)
            //{

            //}
            //else if (gameDifficulty == DifficultyLevel.IronCat)
            //{
            //    currentHealth *= 2;
            //    startHealth *= 2;
            //    health_slider.maxValue = currentHealth;
            //    health_slider.value = currentHealth;
            //}
            //else if (gameDifficulty == DifficultyLevel.Catapocalypse)
            //{
            //    currentHealth *= 3;
            //    startHealth *= 3;
            //    health_slider.maxValue = currentHealth;
            //    health_slider.value = currentHealth;
            //}
        }

    }

    private void Update()
    {
        if (debug)
            TestTakeDamage();

        timeElapsed += Time.deltaTime;
        if (myClass == CharacterClass.Caravan && is_Regenerating) {
            //dmg_percentage = currentHealth % (startHealth / (int)gameDifficulty);
            dmg_percentage = currentHealth % (startHealth / (int)GameManager.DifficultyLevel);
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
        playerMeshRenderer.enabled = true;
        GetComponent<CapsuleCollider>().enabled = true;
        health_slider.gameObject.SetActive(true);
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
        if (animator != null)
        {
            animator.SetTrigger("Die");
            RespawnMethod();
        }
        switch (myClass)
        {
            //case CharacterClass.Player:
            //MusicManager.Instance.PlaySoundTrack(soundCue);
            //break;
            case CharacterClass.Player:
                Debug.Log("Player Dead");
                //if (gameDifficulty == DifficultyLevel.Catfight)
                if (GameManager.DifficultyLevel == DifficultyLevel.Catfight)
                {
                    if (GetComponent<PlayerInventory>() == FindObjectOfType<Goldbag>().holdersInventory)
                    {
                        FindObjectOfType<Goldbag>().DropBag();
                    }
                }
                break;
            case CharacterClass.Caravan:
                Debug.Log("Caravan Dead");
                break;
            case CharacterClass.Obj:
                dropController.DropItem();
                gameObject.SetActive(false);
                break;
            case CharacterClass.Enemy:
                dropController.DropItem();
                ObjectPooler.SetInactive(this.gameObject);
                SpawnManager.EnemiesAlive--;
                break;
        }
    }
    /// <summary>
    /// Handles player respawn when dead
    /// </summary>
    private void RespawnMethod()
    {
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        switch (GameManager.DifficultyLevel)
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
            case DifficultyLevel.Catfight:
                EditorHelper.NotSupportedException("DifficultyLevel.Catfight");
                break;
            default:
                EditorHelper.NotSupportedException("default");
                break;
        }
        //if (gameDifficulty == DifficultyLevel.Normal)
        //{
        //    yield return new WaitForSeconds(4);
        //}
        //else if (gameDifficulty == DifficultyLevel.IronCat)
        //{
        //    yield return new WaitForSeconds(8);
        //}
        //else
        //{
        //    yield return new WaitForSeconds(12);
        //}

        playerMeshRenderer.enabled = false;

        //GetComponent<CapsuleCollider>().enabled = false;
        transform.position = playerSpawnPos.position;

        //make sure its a player first
        if (myClass == CharacterClass.Player)
        {
            GetComponent<PlayerInventory>().RemoveGoldFromInventory(percentageOfGoldToKeep);
        }
        health_slider.gameObject.SetActive(false);
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(6);
        Reset();
        Controller.AddToTargetList(this);
        animator.SetTrigger("Spawn");
        Debug.Log("Respawn");
    }

    /// <summary>
    /// Add to health by some amount
    /// </summary>
    /// <param name="amount"> The amount that will be added to health </param>
    public void AddHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, startHealth);
        DisplayHealth();
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
