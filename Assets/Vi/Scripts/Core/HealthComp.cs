using UnityEngine;
using ObjectPooling;
using UnityEngine.UI;


public enum CharacterClass { Player, Enemy, Caravan, Obj };
public enum DifficultyLevel { Normal = 4, IronCat = 10, Catapocalypse = 25};

public class HealthComp : MonoBehaviour
{
    [SerializeField] private DifficultyLevel gameDifficulty = DifficultyLevel.Normal;
    public CharacterClass myClass;
    public int startHealth = 100;
    public bool debug;
    public int damageTakenPerSecond;

    public SoundClipsInts soundCue = SoundClipsInts.Death;

    [SerializeField]
    private int currentHealth = 0;
    [SerializeField]
    private bool is_Regenerating = false;
    [SerializeField]
    private float dmg_percentage;
    
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
        currentHealth -= amount;
        currentHealth = Mathf.Max(0, currentHealth);
        DisplayHealth();

        if (currentHealth <= 0)
        {
            Dead();
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
        Debug.Log("is dead" + is_Dead);
        if (!is_Dead)
        {
            Debug.Log("calculate damage");
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
        }
        switch (myClass)
        {
            //case CharacterClass.Player:
                //MusicManager.Instance.PlaySoundTrack(soundCue);
                //break;
            case CharacterClass.Player:
                Debug.Log("Player Dead");
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