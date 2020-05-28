using UnityEngine;
using ObjectPooling;
using UnityEngine.UI;
using System;
using System.Collections;
using AI;

public enum CharacterClass { Player, Enemy, Caravan, Obj };
public enum DifficultyLevel { Normal, IronCat, Catapocalypse, Catfight};

public class HealthComp : MonoBehaviour
{
    [SerializeField] private DifficultyLevel gameDifficulty = DifficultyLevel.Normal;
    public CharacterClass myClass;
    public int startHealth = 100;
    public bool debug;
    public int damageTakenPerSecond;
    public GameManager gman;
   
    public SoundClipsInts soundCue = SoundClipsInts.Death;
    public float percentageOfGoldToKeep = 75f;

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

    private float nextDamageTime = 0;
    private float timeElapsed = 0;
    private bool is_Dead = false;
    private Rigidbody rb;
    private DropController dropController;

    public Slider health_slider = null;

    private static ObjectPooler objectPooler;
    Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        dropController = GetComponent<DropController>();
        objectPooler = FindObjectOfType<ObjectPooler>();
        animator = GetComponent<Animator>();
        gman = FindObjectOfType<GameManager>();

        if (FindObjectOfType<GameDifficultyManager>())
        {
            gameDifficulty = FindObjectOfType<GameDifficultyManager>().dif;
        }
        if (myClass == CharacterClass.Enemy)
        {
            //dropController = GetComponent<DropController>();
            //objectPooler = FindObjectOfType<ObjectPooler>();
        }else if(myClass == CharacterClass.Caravan)
        {

        }
        else if(myClass == CharacterClass.Obj)
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
            if (gameDifficulty == DifficultyLevel.Normal)
            {
                percentageOfGoldToKeep = 75f;
            }
            else if (gameDifficulty == DifficultyLevel.IronCat)
            {
                percentageOfGoldToKeep = 50f;
            }
            else if (gameDifficulty == DifficultyLevel.Catapocalypse)
            {
                percentageOfGoldToKeep = 25f;
            }
        }
        else
        {
            //set % of gold to lose based on difficulty
            if (gameDifficulty == DifficultyLevel.Normal)
            {
                
            }
            else if (gameDifficulty == DifficultyLevel.IronCat)
            {
                currentHealth *= 2;
                startHealth *= 2;
                health_slider.maxValue = currentHealth;
                health_slider.value = currentHealth;
            }
            else if (gameDifficulty == DifficultyLevel.Catapocalypse)
            {
                currentHealth *= 3;
                startHealth *= 3;
                health_slider.maxValue = currentHealth;
                health_slider.value = currentHealth;
            }
        }
        
    }

    private void Update()
    {
        if (debug)
            TestTakeDamage();

        timeElapsed += Time.deltaTime;
        if (myClass == CharacterClass.Caravan && is_Regenerating) {
            dmg_percentage = currentHealth % (startHealth / (int)gameDifficulty);
            if (dmg_percentage == 0)
            {
                is_Regenerating = false;
            }
            else { AddHealth(1); }
        }
    
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
        if (!is_Dead)
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
    /// <param name="weapon_force"> The amount of knockback_force from the weapon </param>"
    public void TakeDamage(Transform damageDealer, int amount, float weapon_force)
    {
        if (!is_Dead)
        {
            currentHealth -= amount;
            currentHealth = Mathf.Max(0, currentHealth);
            DisplayHealth();

            KnockBack((damageDealer.position - transform.position) * 2f * weapon_force);

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
        is_Dead = true;
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
                if (gameDifficulty == DifficultyLevel.Catfight)
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

    void RemoveGoldFromInventory()
    {
        //get the inventory to remove gold based on percentage
        GetComponent<PlayerInventory>().gold = Mathf.RoundToInt((float)GetComponent<PlayerInventory>().gold / 100 * percentageOfGoldToKeep);
    }
    private IEnumerator Respawn()
    {
        if (gameDifficulty == DifficultyLevel.Normal)
        {
            yield return new WaitForSeconds(4);
        }
        else if (gameDifficulty == DifficultyLevel.IronCat)
        {
            yield return new WaitForSeconds(8);
        }
        else
        {
            yield return new WaitForSeconds(12);
        }
        
        playerMeshRenderer.enabled = false;
        
        //GetComponent<CapsuleCollider>().enabled = false;
        this.transform.position = playerSpawnPos.position;

        //make sure its a player first
        if (GetComponent<PlayerInventory>())
        {
            RemoveGoldFromInventory();
        }
        health_slider.gameObject.SetActive(false);
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(6);
        is_Dead = false;
        animator.SetTrigger("Spawn");
        currentHealth = startHealth;
        health_slider.value = currentHealth;
        playerMeshRenderer.enabled = true;
        GetComponent<CapsuleCollider>().enabled = true;
        Controller.AddToTargetList(this);
        Debug.Log("Respawn");


       
        health_slider.gameObject.SetActive(true);


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
    /// Returns if character is dead
    /// </summary>
    public bool IsDead()
    {
        return is_Dead;
    }

    public void SetIsDead(bool isDead)
    {
        is_Dead = isDead;
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
        if(health_slider)
        health_slider.value = currentHealth;
    }
}

//#if UNITY_EDITOR
//[CustomEditor(typeof(HealthComp))]
//public class MyScriptEditor : Editor
//{
//    override public void OnInspectorGUI()
//    {
//        var myScript = target as HealthComp;

//        myScript.startHealth = EditorGUILayout.FloatField("Start Health", myScript.startHealth);
//        myScript.debug = GUILayout.Toggle(myScript.debug, "Debug");

//        if (myScript.debug)
//            myScript.damageTakenPerSecond = EditorGUILayout.FloatField("Damage Taken Per Second", myScript.damageTakenPerSecond);

//        myScript.myClass = (CharacterClass)EditorGUILayout.EnumFlagsField(myScript.myClass);

//    }
//}
//#endif